using System;
using System.Collections.Generic;
using BetDotNext.Models;
using BetDotNext.Services;
using BetDotNext.Utils;
using Telegram.Bot.Types;

namespace BetDotNext.Commands
{
  public class BotCommandService
  {
    private static readonly IDictionary<string, Type> Commands = new Dictionary<string, Type>();

    private readonly IServiceProvider _serviceProvider;
    private readonly ConversationService _conversationService;

    public BotCommandService(IServiceProvider serviceProvider, ConversationService botRepository)
    {
      Ensure.NotNull(serviceProvider, nameof(serviceProvider));
      Ensure.NotNull(botRepository, nameof(botRepository));

      _serviceProvider = serviceProvider;
      _conversationService = botRepository;

      Commands.Add(StartCommand.CommandName, typeof(StartCommand));
      Commands.Add(BetCommand.CommandName, typeof(BetCommand));
      Commands.Add(RemoveBetCommand.CommandName, typeof(RemoveBetCommand));
    }

    public CommandBase GetCommand(Message message)
    {
      var command = SearchCommand(message.Text, message.Chat.Id, false);
      if (command != null)
      {
        CreateConversation(message);
        return command;
      }

      var conversation = _conversationService.GetConversationByChatId(message.Chat.Id);
      if (conversation != null)
      {
        return SearchCommand(conversation.Command, message.Chat.Id, true);
      }

      return null;
    }

    private CommandBase SearchCommand(string textCommand, long chatId, bool conversation)
    {
      if (!conversation)
      {
        _conversationService.DeleteConversation(chatId);
      }

      if (Commands.TryGetValue(textCommand, out var type))
      {
        var command = _serviceProvider.GetService(type) as CommandBase;
        return command;
      }

      return null;
    }

    private void CreateConversation(Message message)
    {
      _conversationService.NewConversation(new Conversation
      {
        Chat = message.Chat,
        Command = message.Text,
      });
    }
  }
}