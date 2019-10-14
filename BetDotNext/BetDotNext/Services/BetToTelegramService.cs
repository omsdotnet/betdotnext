using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BetDotNext.Services
{
    public class BetToTelegramService : IHostedService
    {
        private Timer _timer;
        private readonly QueueMessagesService _queueMessagesService;
        
        public BetToTelegramService(QueueMessagesService queueMessagesService)
        {
            _queueMessagesService = queueMessagesService ?? throw new ArgumentNullException(nameof(queueMessagesService));
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}