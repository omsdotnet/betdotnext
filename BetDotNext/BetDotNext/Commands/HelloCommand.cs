using MediatR;

namespace BetDotNext.Commands
{
    public class HelloCommand : IRequest
    {
        internal const string Command = "start";
        
        public int UserId { get; }
        public int MessageId { get; }
        public string UserName { get; }
        
        public HelloCommand(int userId, int messageId, string userName)
        {
            UserId = userId;
            MessageId = messageId;
            UserName = userName;
        }
    }
}