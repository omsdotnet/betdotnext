using System.Threading.Tasks;
using BetDotNext.Services;
using BetDotNext.Utils;
using Telegram.Bot.Types;

namespace BetDotNext.Commands
{
  public abstract class CommandBase
  {
    internal QueueMessagesService QueueMessages { get; }

    protected CommandBase(QueueMessagesService queueMessages)
    {
      Ensure.NotNull(queueMessages, nameof(queueMessages));
      QueueMessages = queueMessages;
    }

    public abstract Task ExecuteAsync(Message message);
  }
}
