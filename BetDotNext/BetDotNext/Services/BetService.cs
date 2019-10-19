using System;
using BetDotNext.Commands;
using BetDotNext.Data;
using BetDotNext.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace BetDotNext.Services
{
    public class BetService
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly UserRepository _userRepository;
        private readonly ILogger<BetService> _logger;
        private readonly IMediator _mediator;
        private readonly ActiveCommandService _activeCommandService;
        
        public BetService(ITelegramBotClient telegramBotClient, UserRepository userRepository,
            ILogger<BetService> logger, IMediator mediator, ActiveCommandService activeCommandService)
        {
            Ensure.NotNull(telegramBotClient, nameof(telegramBotClient));
            Ensure.NotNull(userRepository, nameof(userRepository));
            Ensure.NotNull(logger, nameof(logger));
            Ensure.NotNull(mediator, nameof(mediator));
            Ensure.NotNull(activeCommandService, nameof(activeCommandService));

            _telegramBotClient = telegramBotClient;
            _userRepository = userRepository;
            _logger = logger;
            _mediator = mediator;
            _activeCommandService = activeCommandService;
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
            _logger.LogInformation(message.ToJson());

            if (message?.Chat == null)
            {
                return;
            }
            
            MessageHandler(message);
        }

        private void MessageHandler(Message message)
        {
            var chatId = message.Chat.Id;
            var user = _userRepository.GetUserByChatId(chatId);

            if (user == null)
            {
                _telegramBotClient.SendTextMessageAsync(message.Chat, "Test Message").Wait();
                // _mediator.Send(new HelloCommand(chatId, message.MessageId, message.From.Username));
                return;
            }

            if (message.Text.Contains(BetCommand.Command))
            {
                _mediator.Send(new BetCommand());
                return;
            }

            if (message.Text.Contains(RuleCommand.Command))
            {
                _mediator.Send(new RuleCommand());
                return;
            }

            if (message.Text.Contains(MyBetsCommand.Command))
            {
                _mediator.Send(new MyBetsCommand());
            }
        }
    }
}