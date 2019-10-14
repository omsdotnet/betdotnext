using System.Collections.Generic;
using System.Linq;
using BetDotNext.Models;
using BetDotNext.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BetDotNext.Services
{
    public class QueueMessagesService
    {
        private readonly IMongoCollection<MessageQueue> _messageQueue;
        private readonly IMongoQueryable<MessageQueue> _queryable;

        public QueueMessagesService(IMongoCollection<MessageQueue> messageQueue)
        {
            Ensure.NotNull(messageQueue, nameof(messageQueue));
            
            _messageQueue = messageQueue;
            _queryable = messageQueue.AsQueryable();
        }

        public void Enqueue(MessageQueue messageQueue)
        {
            _messageQueue.InsertOne(messageQueue);
        }
        
        public void Dequeue(ObjectId id)
        {
            _messageQueue.FindOneAndDelete(new BsonDocument("_id", id));
        }

        public IEnumerable<MessageQueue> TopMessages(int limit)
        {
            return Enumerable.Empty<MessageQueue>();
        }
    }
}