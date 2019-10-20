using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using BetDotNext.Utils;

namespace BetDotNext.ExternalServices
{
  public class BetPlatformService
  {
    public readonly HttpClient _httpClient;

    public BetPlatformService(HttpClient httpClient)
    {
      Ensure.NotNull(httpClient, nameof(httpClient));

      _httpClient = httpClient;

      Authentication().Wait();
    }

    private async Task<bool> Authentication()
    {
      var url = $"api/bidders/GetAll";
      var result = await _httpClient.GetAsync(url);
      var str = await result.Content.ReadAsStringAsync();
      return result.IsSuccessStatusCode;
    }


  }
}
