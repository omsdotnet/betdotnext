using System;
using BetDotNext.Commands;
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
    private readonly BotCommandService _botCommandService;

    public BetService(ITelegramBotClient telegramBotClient,
      ILogger<BetService> logger,
      BotCommandService botCommandService)
    {
      Ensure.NotNull(telegramBotClient, nameof(telegramBotClient));
      Ensure.NotNull(logger, nameof(logger));
      Ensure.NotNull(botCommandService, nameof(botCommandService));

      _telegramBotClient = telegramBotClient;
      _logger = logger;
      _botCommandService = botCommandService;
    }

    public void Start()
    {
      try
      {
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
      var chatId = message.Chat.Id;
      CommandBase commandBase = _botCommandService.GetCommand(message);
      if (commandBase != null)
      {
        await commandBase.ExecuteAsync(message);
      }
    }
  }
}