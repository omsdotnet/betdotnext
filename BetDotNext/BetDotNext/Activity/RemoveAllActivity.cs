using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using Telegram.Bot.Types;

namespace BetDotNext.Activity
{
  public class RemoveAllActivity : BotActivityBase
  {
    public RemoveAllActivity(IBotStorage botStorage, IBotMediator mediator) : base(botStorage, mediator)
    {
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      return Task.FromResult(true);
    }
  }
}
