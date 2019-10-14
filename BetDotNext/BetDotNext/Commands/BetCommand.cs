using MediatR;

namespace BetDotNext.Commands
{
    public class BetCommand : IRequest
    {
        internal const string Command = "newbet";
    }
}