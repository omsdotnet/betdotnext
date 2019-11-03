using System.Threading.Tasks;
using BetDotNext.BotPlatform;
using BetDotNext.BotPlatform.Impl;
using BetDotNext.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace BetDotNext.Activity
{
  public class ScoreActivity : BotActivityBase
  {
    private readonly QueueMessagesService _queueMessagesService;
    private readonly ILogger<ScoreActivity> _logger;

    public ScoreActivity(IBotStorage botStorage, IBotMediator mediator,
      QueueMessagesService queueMessagesService, ILogger<ScoreActivity> logger) : base(botStorage, mediator)
    {
      _queueMessagesService = queueMessagesService;
      _logger = logger;
    }

    public override BotActivityBase SelectActivity<T>(Message message, T context) => null;

    public override Task<bool> CurrentExecuteAsync<T>(Message message, T context)
    {
      return Task.FromResult(true);
    }
  }
}
