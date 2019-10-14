using System.Threading;
using System.Threading.Tasks;
using BetDotNext.Commands;
using MediatR;

namespace BetDotNext.Handlers
{
    public class HelloHandler : IRequestHandler<HelloCommand>
    {
        public Task<Unit> Handle(HelloCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Unit());
        }
    }
}