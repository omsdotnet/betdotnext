using System;
using BetDotNext.BotPlatform;
using BetDotNext.Utils;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace BetDotNext.Services
{
  public class BetService
  {
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<BetService> _logger;
    private readonly IBot _bot;

    public BetService(ITelegramBotClient telegramBotClient,
      ILogger<BetService> logger, IBot bot)
    {
      Ensure.NotNull(telegramBotClient, nameof(telegramBotClient));
      Ensure.NotNull(logger, nameof(logger));
      Ensure.NotNull(bot, nameof(bot));

      _telegramBotClient = telegramBotClient;
      _logger = logger;
      _bot = bot;
    }

    public void Start()
    {
      try
      {
        _logger.LogInformation("Bot Started");
        if (_telegramBotClient.IsReceiving)
        {
          return;
        }

        _telegramBotClient.OnMessage += OnMessageReceive;
        _telegramBotClient.StartReceiving();
      }
      catch (ApiRequestException ex)
      {
        _logger.LogCritical($"Message: {ex.Message}");
        throw;
      }
    }

    public void Stop()
    {
      try
      {
        _logger.LogInformation("Bot Stopped");
        _telegramBotClient.OnMessage -= OnMessageReceive;
        _telegramBotClient.StopReceiving();
      }
      catch (Exception ex)
      {
        _logger.LogCritical($"Message: {ex.Message}");
        throw;
      }
    }

    private void OnMessageReceive(object sender, MessageEventArgs args)
    {
      Message message = args.Message;
      if (message?.Chat == null)
      {
        return;
      }

      MessageHandler(message);
    }

    private async void MessageHandler(Message message)
    {
      try
      {
        string user = !string.IsNullOrEmpty(message.Chat.Username) ?
          message.Chat.Username :
          $"{message.Chat.LastName} {message.Chat.FirstName}";

        _logger.LogInformation("Received message {0} from chat {1} user {2}. ", 
          message.Text, 
          message.Chat.Id, 
          user);

        await _bot.StartDialogAsync(message);
      }
      catch (Exception ex)
      {
        _logger.LogCritical(ex, "The message cannot be processed.");
      }
    }
  }
}