using System.Threading.Tasks;
using BetDotNext.Services;
using Telegram.Bot.Types;

namespace BetDotNext.Commands
{
  public class RemoveBetCommand : CommandBase
  {
    public const string CommandName = "/removebet";
    public RemoveBetCommand(QueueMessagesService queueMessages) : base(queueMessages)
    {
    }

    public override Task ExecuteAsync(Message message)
    {
      var k = "";

      return Task.CompletedTask;
    }
  }
}
