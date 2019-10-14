using System;
using BetDotNext.Commands;
using BetDotNext.Data;
using MediatR;
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
        private readonly UserRepository _userRepository;
        private readonly ILogger<BetService> _logger;
        private readonly IMediator _mediator;
        
        public BetService(ITelegramBotClient telegramBotClient, UserRepository userRepository,
            ILogger<BetService> logger, IMediator mediator)
        {
            _telegramBotClient = telegramBotClient ?? throw new ArgumentNullException(nameof(telegramBotClient));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
            if (message == null || message.From == null)
            {
                return;
            }

            if (message.From.IsBot)
            {
                return;
            }
            
            MessageHandler(message);
        }

        private void MessageHandler(Message message)
        {
            int userId = message.From.Id;
            var user = _userRepository.GetUserById(userId);

            if (user == null)
            {
                _mediator.Send(new HelloCommand(userId, message.MessageId, message.From.Username));
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