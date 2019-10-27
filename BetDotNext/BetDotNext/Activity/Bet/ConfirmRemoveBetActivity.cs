using System;
using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using Telegram.Bot.Types;

namespace BetDotNext.Activity.Bet
{
  public class ConfirmRemoveBetActivity : BotActivityBase
  {
    public ConfirmRemoveBetActivity(IBotStorage botStorage) : base(botStorage)
    {
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      return Task.FromResult(true);
    }
  }
}
