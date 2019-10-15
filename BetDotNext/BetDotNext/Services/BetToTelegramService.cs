using System;
using System.Threading;
using System.Threading.Tasks;
using BetDotNext.Utils;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace BetDotNext.Services
{
    internal class BetToTelegramService : IHostedService
    {
        private readonly QueueMessagesService _queueMessagesService;
        private readonly ITelegramBotClient _telegramBotClient;
        
        public BetToTelegramService(QueueMessagesService queueMessagesService, ITelegramBotClient telegramBotClient)
        {
            Ensure.NotNull(queueMessagesService, nameof(queueMessagesService));
            Ensure.NotNull(telegramBotClient, nameof(telegramBotClient));
            
            _queueMessagesService = queueMessagesService;
            _telegramBotClient = telegramBotClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var message in _queueMessagesService.TopMessages(30))
                {
                    await _telegramBotClient.SendTextMessageAsync(0, message.Text, cancellationToken: cancellationToken);
                    _queueMessagesService.Dequeue(message.Id);
                }
                
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}