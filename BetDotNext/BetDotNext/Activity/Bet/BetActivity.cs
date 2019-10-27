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
  public class BetActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;
    private readonly BetPlatformService _betPlatformService;

    public BetActivity(IBotStorage botStorage, QueueMessagesService queueMessagesService, 
      BetPlatformService betPlatformService) : base(botStorage)
    {
      _queueMessagesService = queueMessagesService;
      _betPlatformService = betPlatformService;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context)
    {
      return new CreatedBetActivity(BotStorage, _queueMessagesService, _betPlatformService);
    }

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      var newMessage = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.AcceptedBetActivityMessage };
      _queueMessagesService.Enqueue(newMessage);

      return Task.FromResult(true);
    }
  }
}
