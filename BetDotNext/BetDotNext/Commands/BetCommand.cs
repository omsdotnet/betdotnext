using System;
using System.Threading.Tasks;
using BetDotNext.ExternalServices;
using BetDotNext.Models;
using BetDotNext.Services;
using BetDotNext.Utils;
using Telegram.Bot.Types;

namespace BetDotNext.Commands
{
  public class BetCommand : CommandBase
  {
    public const string CommandName = "/bet";

    private readonly BetPlatformService _betPlatform;

    public BetCommand(QueueMessagesService messagesService, BetPlatformService betPlatform) : base(messagesService)
    {
      Ensure.NotNull(betPlatform, nameof(betPlatform));
      _betPlatform = betPlatform;
    }

    public override Task ExecuteAsync(Message message)
    {
      if (message.Text != CommandName)
      {
        string[] parts = message.Text.Split('-', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 3)
        {
          var speaker = parts[0].Trim();
          var rate = parts[1].Trim();
          var category = parts[2].Trim();


        }
        else
        {
          WrongFormatData(message);
        }
      }
      else
      {
        CreateBetMessage(message);
      }

      return Task.CompletedTask;
    }

    private void CreateBetMessage(Message message)
    {
      QueueMessages.Enqueue(new MessageQueue
      {
        Text = MessageConstants.BetCreateMessage,
        StartTime = DateTime.UtcNow,
        MessageId = message.MessageId,
        Chat = message.Chat
      });
    }

    private void WrongFormatData(Message message)
    {
      QueueMessages.Enqueue(new MessageQueue
      {
        Text = MessageConstants.WrongCreateDataMessage,
        StartTime = DateTime.UtcNow,
        MessageId = message.MessageId,
        Chat = message.Chat
      });
    }
  }
}
