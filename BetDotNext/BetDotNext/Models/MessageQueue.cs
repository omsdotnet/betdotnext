using MongoDB.Bson;
using Telegram.Bot.Types.ReplyMarkups;

namespace BetDotNext.Models
{
    public class MessageQueue
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup { get; set; }
    }
}