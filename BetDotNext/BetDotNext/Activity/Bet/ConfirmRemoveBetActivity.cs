using System;
using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.ExternalServices;
using BetDotNext.Models;
using BetDotNext.Services;
using Telegram.Bot.Types;

namespace BetDotNext.Activity.Bet
{
  public class ConfirmRemoveBetActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;
    private readonly BetPlatformService _betPlatformService;

    public ConfirmRemoveBetActivity(IBotStorage botStorage, QueueMessagesService queueMessagesService,
      BetPlatformService betPlatformService) : base(botStorage)
    {
      _queueMessagesService = queueMessagesService;
      _betPlatformService = betPlatformService;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override async Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      string[] parts = message.Text.Split('-', StringSplitOptions.RemoveEmptyEntries);
      int len = parts.Length;
      if (len != 2 || len != 3)
      {
        var err = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.BetActivityUnexpectedFormatMessage };
        _queueMessagesService.Enqueue(err);
        return false;
      }

      try
      {
        var b = new CreateBet { Bidder = parts[0].Trim() };
        switch (len)
        {
          case 2:
            b.Rate = uint.Parse(parts[1].Trim());
            break;
          case 3:
            b.Speaker = parts[1].Trim();
            b.Rate = uint.Parse(parts[2].Trim());
            break;
        }

        await _betPlatformService.DeleteRateForBet(b);
      }
      catch (Exception)
      {
        return false;
      }

      //_betPlatformService.

      return true;
    }
  }
}
