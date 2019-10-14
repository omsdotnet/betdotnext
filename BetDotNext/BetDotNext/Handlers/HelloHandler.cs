using System.Threading;
using System.Threading.Tasks;
using BetDotNext.Commands;
using MediatR;

namespace BetDotNext.Handlers
{
    public class HelloHandler : IRequestHandler<HelloCommand>
    {
        public async Task<Unit> Handle(HelloCommand request, CancellationToken cancellationToken)
        {
            await Task.FromResult(new Unit());
            return Unit.Value;
        }
    }
}