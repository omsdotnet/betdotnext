using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BetDotNext.Activity;
using BetDotNext.ExternalServices.Dto;
using BetDotNext.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BetDotNext.ExternalServices
{
  public class BetPlatformService
  {
    private const string TeamUrl = "api/teams/GetAll";
    private const string BiddersUrl = "api/bidders/GetAll";
    private const string RidesUrl = "api/rides/GetAll";
    private const string RideByIdUrl = "api/rides/GetById/";

    private readonly HttpClient _httpClient;
    private readonly ILogger<BetPlatformService> _logger;

    private IList<Bidder> _bidders;
    private IList<Team> _teams;
    private IList<Ride> _rides;

    public BetPlatformService(HttpClient httpClient, ILogger<BetPlatformService> logger)
    {
      Ensure.NotNull(httpClient, nameof(httpClient));

      _httpClient = httpClient;
      _logger = logger;
    }

    public async Task InitAsync()
    {
      try
      {
        await AuthenticationAsync();
        _bidders = await BiddersAsync();
        _teams = await TeamsAsync();
        _rides = await RidesAsync();
      }
      catch (Exception ex)
      {
        _logger.LogCritical("An exception was thrown during initialization dictionary: {0}", ex.Message);
      }
    }

    private async Task AuthenticationAsync()
    {
      var url = $"api/Authentication/Login?login={HttpUtility.UrlEncode("Азино")}&password={HttpUtility.UrlEncode("3топора3")}";
      var result = await _httpClient.PostAsync(url, null);
      var _ = await result.Content.ReadAsStringAsync();
      result.EnsureSuccessStatusCode();
    }

    private async Task<IList<Bidder>> BiddersAsync()
    {
      var result = await _httpClient.GetAsync(BiddersUrl);
      var s = await result.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<IList<Bidder>>(s);
    }

    private async Task<IList<Team>> TeamsAsync()
    {
      var result = await _httpClient.GetAsync(TeamUrl);
      var s = await result.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<IList<Team>>(s);
    }

    private async Task<IList<Ride>> RidesAsync()
    {
      var result = await _httpClient.GetAsync(RidesUrl);
      var s = await result.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<IList<Ride>>(s);
    }

    private async Task<Ride> GetRide(uint id)
    {
      var result = await _httpClient.GetAsync(RideByIdUrl + id);
      var biddersStr = await result.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<Ride>(biddersStr);
    }

    private async Task UpdateRide(Ride item)
    {
      string jsonString = JsonConvert.SerializeObject(item);
      using HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
      var result = await _httpClient.PutAsync("api/rides/" + item.Id, httpContent);
      result.EnsureSuccessStatusCode();
    }

    private async Task AddBidder(Bidder bidder)
    {
      string jsonString = JsonConvert.SerializeObject(bidder);
      using HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
      var result = await _httpClient.PostAsync("api/Bidders", httpContent);
      result.EnsureSuccessStatusCode();
    }

    public async Task<long?> CreateBetAsync(CreateBet bet)
    {
      if (_bidders == null || _teams == null || _rides == null)
      {
        await InitAsync();
      }
      else
      {
        await AuthenticationAsync();
      }

      var bidder = _bidders.SingleOrDefault(x => x.Name.ToLower() == bet.Bidder.ToLower());
      if (bidder == null)
      {
        _logger.LogDebug("Created new bidder {0}", bet.Bidder);

        var bidderId = !_bidders.Any() ? 0 : _bidders.Max(p => p.Id);
        var newBidder = new Bidder { Id = ++bidderId, Name = bet.Bidder, CurrentScore = 0, StartScore = 1000 };
        await AddBidder(newBidder);
        _bidders = await BiddersAsync();
      }

      var speaker = _teams.SingleOrDefault(x => x.Name.ToLower().Replace('-', ' ') == bet.Speaker.ToLower());
      if (speaker == null)
      {
        _logger.LogError("Not found speaker");
        return null;
      }

      var rideId = _rides.SingleOrDefault(x => x.Number == bet.Ride)?.Id;
      if (!rideId.HasValue)
      {
        _logger.LogError("Not found ride");
        return null;
      }

      var ride = await GetRide(rideId.Value);
      var rate = ride.Rates.SingleOrDefault(x => x.Bidder.Id == bidder.Id && x.Team == speaker.Id);
      if (bet.Rate == 0)
      {
        if (rate != null)
        {
          ride.Rates.Remove(rate);
        }
      }
      else if (rate != null)
      {
        if (bidder?.CurrentScore + rate.RateValue < bet.Rate)
        {
          _logger.LogError($"rate: {bidder.CurrentScore} < {bet.Rate}");
          return null;
        }

        rate.RateValue = bet.Rate;
      }
      else
      {
        if (bidder?.CurrentScore < bet.Rate)
        {
          _logger.LogError($"rate: {bidder.CurrentScore} < {bet.Rate}");
          return null;
        }

        var maxId = ride.Rates.Any() ? ride.Rates.Max(x => x.Id) : 0;
        var rateItem = new Rate
        {
          Id = ++maxId,
          Bidder = bidder,
          RateValue = bet.Rate,
          Team = speaker.Id
        };

        ride.Rates.Add(rateItem);
      }

      await UpdateRide(ride);

      _bidders = await BiddersAsync();
      _rides = await RidesAsync();

      return _bidders.Single(x => x.Id == bidder?.Id).CurrentScore;
    }

    public async Task<string> DeleteRateForBet(CreateBet bet)
    {
      if (_bidders == null || _teams == null || _rides == null)
      {
        await InitAsync();
      }
      else
      {
        await AuthenticationAsync();
      }

      if (bet.Rate != 0)
      {
        _logger.LogError($"ERROR - rate: {bet.Rate} != 0");
        return "Ставка должна быть равна 0.";
      }

      var bidder = _bidders.SingleOrDefault(x => x.Name.ToLower() == bet.Bidder.ToLower());
      if (bidder == null)
      {
        _logger.LogError("ERROR - bidder not found");
        return null;
      }

      var isDeleteForSpeaker = !string.IsNullOrEmpty(bet.Speaker);
      Team speaker = null;
      if (isDeleteForSpeaker)
      {
        speaker = _teams.SingleOrDefault(x => x.Name.ToLower().Replace('-', ' ') == bet.Speaker.ToLower());
        if (speaker == null)
        {
          _logger.LogError("ERROR - bidder not found");
          return StringsResource.BetRateNotEquelsMessage;
        }
      }

      var ridesToUpdate = _rides
        .Where(x => x.Rates.Any(y => y.Bidder.Id == bidder.Id && (!isDeleteForSpeaker || y.Team == speaker?.Id)))
        .Select(x => x.Id)
        .ToList();

      foreach (var rideId in ridesToUpdate)
      {
        var ride = await GetRide(rideId);

        var ratesToDelete = ride.Rates
          .Where(x => x.Bidder.Id == bidder.Id && x.Team == speaker.Id)
          .ToList();

        foreach (var rateToDelete in ratesToDelete)
        {
          ride.Rates.Remove(rateToDelete);
        }

        await UpdateRide(ride);
      }


      _bidders = await BiddersAsync();
      _rides = await RidesAsync();

      var currentScore = _bidders.Single(x => x.Id == bidder.Id).CurrentScore;
      _logger.LogInformation("Current score a participant {0} = {1} from", bet.Bidder, currentScore);
      return $"Ваша текущая ставка {currentScore}";
    }
  }
}
