using System;
using System.Threading;
using System.Threading.Tasks;
using BetDotNext.Utils;
using Microsoft.Extensions.Hosting;

namespace BetDotNext.Services
{
    internal class BetToTelegramService : IHostedService
    {
        private readonly Timer _timer;
        private readonly QueueMessagesService _queueMessagesService;
        
        public BetToTelegramService(QueueMessagesService queueMessagesService)
        {
            Ensure.NotNull(queueMessagesService, nameof(queueMessagesService));
            
            _queueMessagesService = queueMessagesService;
            _timer = new Timer(OnTick, null, 0, 0);
        }

        private void OnTick(object state)
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _timer.DisposeAsync();
        }
    }
}