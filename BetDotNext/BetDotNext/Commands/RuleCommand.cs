using MediatR;

namespace BetDotNext.Commands
{
    public class RuleCommand: IRequest
    {
        internal const string Command = "rules";
    }
}