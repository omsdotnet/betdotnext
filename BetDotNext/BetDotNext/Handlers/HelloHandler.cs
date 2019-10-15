using System;
using BetDotNext.Commands;
using BetDotNext.Models;
using BetDotNext.Services;
using BetDotNext.Utils;
using MediatR;

namespace BetDotNext.Handlers
{
    public class HelloHandler : RequestHandler<HelloCommand>
    {
        private readonly QueueMessagesService _queueMessagesService;

        public HelloHandler(QueueMessagesService queueMessagesService)
        {
            Ensure.NotNull(queueMessagesService, nameof(queueMessagesService));

            _queueMessagesService = queueMessagesService;
        }

        protected override void Handle(HelloCommand request)
        {
            _queueMessagesService.Enqueue(new MessageQueue
            {
                Text = "Hello!!! I am a bot",
                StartTime = DateTime.UtcNow,
                MessageId = request.MessageId,
                ChatId =  request.ChatId
            });
        }
    }
}