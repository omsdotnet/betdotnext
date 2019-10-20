using BetDotNext.Models;
using BetDotNext.Utils;
using MongoDB.Driver;

namespace BetDotNext.Services
{
  public class ConversationService
  {
    private readonly IMongoCollection<Conversation> _conversation;

    public ConversationService(IMongoDatabase mongoDatabase)
    {
      Ensure.NotNull(mongoDatabase, nameof(mongoDatabase));

      _conversation = mongoDatabase.GetCollection<Conversation>(typeof(Conversation).Name);
    }

    public Conversation GetConversationByChatId(long chatId)
    {
      var filterByChatId = Builders<Conversation>.Filter.Eq(p => p.Chat.Id, chatId);
      return _conversation.Find(filterByChatId).FirstOrDefault();
    }

    public void NewConversation(Conversation conversation)
    {
      _conversation.InsertOne(conversation);
    }

    public void DeleteConversation(long chatId)
    {
      var filterByChatId = Builders<Conversation>.Filter.Eq(p => p.Chat.Id, chatId);
      _conversation.DeleteOne(filterByChatId);
    }
  }
}
