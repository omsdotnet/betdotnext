using System;
using System.Threading.Tasks;
using BetDotNext.Models;
using BetDotNext.Services;
using Telegram.Bot.Types;

namespace BetDotNext.Commands
{
  public class StartCommand : CommandBase
  {
    public const string CommandName = "/start";

    public StartCommand(QueueMessagesService messagesService) : base(messagesService)
    {
    }

    public override Task ExecuteAsync(Message message)
    {
      QueueMessages.Enqueue(new MessageQueue
      {
        Text = $"Привет {message.Chat.Username ?? "человек без username`a"}. Я бот который принимает ставки на DotNext Moskow 2019.",
        StartTime = DateTime.UtcNow,
        MessageId = message.MessageId,
        Chat = message.Chat
      });

      return Task.CompletedTask;
    }
  }
}
