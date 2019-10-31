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
  public class ConfirmRemoveBetActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;
    private readonly BetPlatformService _betPlatformService;
    private readonly ILogger<ConfirmRemoveBetActivity> _logger;

    public ConfirmRemoveBetActivity(IBotStorage botStorage, IBotMediator mediator, 
      QueueMessagesService queueMessagesService, BetPlatformService betPlatformService,
      ILogger<ConfirmRemoveBetActivity> logger) : base(botStorage, mediator)
    {
      _queueMessagesService = queueMessagesService;
      _betPlatformService = betPlatformService;
      _logger = logger;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override async Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      var chatId = message.Chat.Id;

      var parts = message.Text.Split('-', StringSplitOptions.RemoveEmptyEntries);
      var len = parts.Length;
      if (len != 2 || len != 3)
      {
        var err = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.BetActivityUnexpectedFormatMessage };
        _queueMessagesService.Enqueue(err);
        _logger.LogWarning("Wrong format when remove a bet from chat {0}", chatId);

        return false;
      }

      var fail = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.FailDeleteActivityMessage };

      try
      {
        _logger.LogInformation("The correct format when delete a bet. Delete a bet.");
        var lm = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = StringsResource.LoadingMessage };
        _queueMessagesService.Enqueue(lm);

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

        var deleteMessage = await _betPlatformService.DeleteRateForBet(b);

        var successfullyMessage = string.IsNullOrWhiteSpace(deleteMessage) ? 
          StringsResource.SuccessfullyRemoveMessage : 
          StringsResource.SuccessfullyRemoveMessage + Environment.NewLine + deleteMessage;

        var success = new MessageQueue { Chat = message.Chat, StartTime = DateTime.UtcNow, Text = successfullyMessage };
        _queueMessagesService.Enqueue(success);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message, "Error when delete a bet.");
        _queueMessagesService.Enqueue(fail);

        return false;
      }

      return true;
    }
  }
}
