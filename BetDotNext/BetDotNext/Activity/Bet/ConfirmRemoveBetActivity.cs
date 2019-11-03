using System;
using System.Linq;
using System.Threading.Tasks;
using BetDotNext.Activity.Utils;
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
      var messageText = message.Text;

      if (messageText == "0")
      {
        return await DeleteAllRate(message);
      }

      var parts = messageText.Split('-', StringSplitOptions.RemoveEmptyEntries);
      var len = parts.Length;
      if (!new[] { 2, 3 }.Contains(len))
      {
        var err = new MessageQueue { Chat = message.Chat, Text = StringsResource.BetActivityUnexpectedFormatMessage };
        _queueMessagesService.Enqueue(err);
        _logger.LogWarning("Wrong format when remove a bet from chat {0}", chatId);

        return false;
      }

      var fail = new MessageQueue { Chat = message.Chat, Text = StringsResource.FailDeleteActivityMessage };

      try
      {
        _logger.LogInformation("The correct format when delete a bet. Delete a bet.");
        var lm = new MessageQueue { Chat = message.Chat, Text = StringsResource.LoadingMessage };
        _queueMessagesService.Enqueue(lm);

        var b = new CreateBet { Bidder = message.GetUserNameOrLastFirstName() };
        switch (len)
        {
          case 2:
            b.Speaker = parts[0].Trim();
            b.Rate = uint.Parse(parts[1].Trim());
            break;
          case 3:
            b.Speaker = parts[0].Trim();
            b.Rate = uint.Parse(parts[1].Trim());
            b.Ride = parts[2].NormalizeRideValue();
            break;
        }

        var deleteMessage = await _betPlatformService.DeleteRateForBetAsync(b);

        var successfullyMessage = string.IsNullOrWhiteSpace(deleteMessage)
          ? StringsResource.SuccessfullyRemoveMessage
          : StringsResource.SuccessfullyRemoveMessage + Environment.NewLine + deleteMessage;

        var success = new MessageQueue { Chat = message.Chat, Text = successfullyMessage };
        _queueMessagesService.Enqueue(success);
      }
      catch (Exception ex) when (ex is UnexpectedFormatMessageException)
      {
        _logger.LogError(ex, "Error when delete a bet.");

        var formatMessage = new MessageQueue { Chat = message.Chat, Text = ex.Message };
        _queueMessagesService.Enqueue(formatMessage);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message, "Error when delete a bet.");
        _queueMessagesService.Enqueue(fail);

        return false;
      }

      return true;
    }

    private async Task<bool> DeleteAllRate(Message message)
    {
      try
      {
        var res = await _betPlatformService.DeleteAllBetAsync(message.GetUserNameOrLastFirstName());

        var messageSuccess = new MessageQueue { Chat = message.Chat, Text = res };
        _queueMessagesService.Enqueue(messageSuccess);

        return true;
      }
      catch (Exception ex) when (ex is UnexpectedFormatMessageException)
      {
        _logger.LogError(ex, "Error when delete a bet.");

        var formatMessage = new MessageQueue { Chat = message.Chat, Text = ex.Message };
        _queueMessagesService.Enqueue(formatMessage);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error when delete a bet.");

        var fail = new MessageQueue { Chat = message.Chat, Text = StringsResource.FailDeleteActivityMessage };
        _queueMessagesService.Enqueue(fail);
      }

      return false;
    }
  }
}
