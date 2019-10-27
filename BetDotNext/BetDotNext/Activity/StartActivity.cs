using System;
using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.Models;
using BetDotNext.Services;
using Telegram.Bot.Types;

namespace BetDotNext.Activity
{
  public class StartActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;

    public StartActivity(IBotStorage botStorage, QueueMessagesService queueMessagesService) : base(botStorage)
    {
      _queueMessagesService = queueMessagesService;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      var newMessage = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.StartActivityMessage };
      _queueMessagesService.Enqueue(newMessage);

      return Task.FromResult(false);
    }
  }
}
