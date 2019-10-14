using MediatR;

namespace BetDotNext.Commands
{
    public class MyBetsCommand : IRequest
    {
        internal const string Command = "mybets";
    }
}