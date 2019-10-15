using MediatR;

namespace BetDotNext.Commands
{
    public class HelloCommand : IRequest
    {
        internal const string Command = "start";
        
        public long ChatId { get; }
        public int MessageId { get; }
        public string UserName { get; }
        
        public HelloCommand(long chatId, int messageId, string userName)
        {
            ChatId = chatId;
            MessageId = messageId;
            UserName = userName;
        }
    }
}