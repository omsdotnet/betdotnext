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
  public class RemoveBetActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;
    private readonly BetPlatformService _betPlatformService;

    public RemoveBetActivity(IBotStorage botStorage, QueueMessagesService queueMessagesService,
      BetPlatformService betPlatformService) : base(botStorage)
    {
      _queueMessagesService = queueMessagesService;
      _betPlatformService = betPlatformService;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context)
    {
      return new ConfirmRemoveBetActivity(BotStorage, _queueMessagesService, _betPlatformService);
    }

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      var m = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.RemoveBetActivityMessage };
      _queueMessagesService.Enqueue(m);
      return Task.FromResult(true);
    }
  }
}
