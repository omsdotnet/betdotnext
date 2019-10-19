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

    private Timer _queueTimer;

    public BetToTelegramService(QueueMessagesService queueMessagesService, ITelegramBotClient telegramBotClient)
    {
      Ensure.NotNull(queueMessagesService, nameof(queueMessagesService));
      Ensure.NotNull(telegramBotClient, nameof(telegramBotClient));

      _queueMessagesService = queueMessagesService;
      _telegramBotClient = telegramBotClient;
    }

    private async void OnExecuteQueueMessages(object state)
    {
      foreach (var message in _queueMessagesService.TopMessages(30))
      {
        await _telegramBotClient.SendTextMessageAsync(0, message.Text);
        _queueMessagesService.Dequeue(message.Id);
      }

      await Task.Delay(TimeSpan.FromSeconds(1.5));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _queueTimer = new Timer(OnExecuteQueueMessages, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
      return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
      await _queueTimer.DisposeAsync();
    }
  }
}