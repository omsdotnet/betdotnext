using System;
using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.ExternalServices;
using BetDotNext.Models;
using BetDotNext.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace BetDotNext.Activity.Bet
{
  public class CreatedBetActivity : BotActivityBase
  {
    private const int SegmentLength = 3;
    private readonly QueueMessagesService _queueMessagesService;
    private readonly BetPlatformService _betPlatformService;
    private readonly ILogger<CreatedBetActivity> _logger;

    public CreatedBetActivity(IBotStorage botStorage, IBotMediator mediator,
      QueueMessagesService queueMessagesService, BetPlatformService betPlatformService,
      ILogger<CreatedBetActivity> logger) : base(botStorage, mediator)
    {
      _queueMessagesService = queueMessagesService;
      _betPlatformService = betPlatformService;
      _logger = logger;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override async Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      long chatId = message.Chat.Id;

      string[] parts = message.Text.Split('-', StringSplitOptions.RemoveEmptyEntries);
      if (parts.Length != SegmentLength)
      {
        var err = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.BetActivityUnexpectedFormatMessage };
        _queueMessagesService.Enqueue(err);
        _logger.LogWarning("Wrong format message from a chat {0}", chatId);

        return false;
      }

      _logger.LogInformation("The correct format message from a chat {0}", chatId);

      var lm = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.LoadingMessage };
      _queueMessagesService.Enqueue(lm);

      var bidder = !string.IsNullOrEmpty(message.Chat.Username) ? 
        message.Chat.Username : 
        $"{message.Chat.LastName} {message.Chat.FirstName}";

      var speaker = parts[0].Trim();
      var rate = parts[1].Trim();
      var ride = NormalizeRideValue(parts[2]);

      var fail = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.FailCreatedActivityMessage };

      try
      {
        var bet = new CreateBet {  Rate = uint.Parse(rate), Ride = ride, Speaker = speaker, Bidder = bidder };
        var currentScore = await _betPlatformService.CreateBetAsync(bet);
        if (!currentScore.HasValue)
        {
          _logger.LogInformation("Fail created bet from a chat {0}", chatId);
          _queueMessagesService.Enqueue(fail);
          return false;
        }

        var tSuccess = string.Format(StringsResource.SuccessBetActivity, currentScore);
        var success = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = tSuccess };
        _queueMessagesService.Enqueue(success);
        _logger.LogInformation("Bet successfully created from a chat {0}", chatId);

        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error when create bets");
        _queueMessagesService.Enqueue(fail);
        
        return false;
      }
    }

    private static uint NormalizeRideValue(string rideValue)
    {
      var rideNumber = rideValue
        .Trim()
        .ToLower()
        .Replace("top3", "11")
        .Replace("top5", "12")
        .Replace("top10", "13");

      return uint.Parse(rideNumber);
    }
  }
}
