using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BetDotNext.Models
{
    public class MessageQueue
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public DateTime StartTime { get; set; }
        public int MessageId { get; set; }
        public ChatId ChatId { get; set; }
        public IReplyMarkup ReplyMarkup { get; set; }
    }
}