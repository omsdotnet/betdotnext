using BetDotNext.Models;
using MongoDB.Driver;

namespace BetDotNext.Setup
{
    public static class MongoDatabaseSetup
    {
        private const string QueueMessage = "QueueMessage";
        public static IMongoDatabase MongoDbInit(this IMongoDatabase mongoDatabase, string adminPass, string userPass)
        {
            var queue = mongoDatabase.GetCollection<MessageQueue>(QueueMessage);
            
            var optionsUnique = new CreateIndexOptions
            {
                Unique = true,
                Background = true
            };

            var optionsBackground = new CreateIndexOptions
            {
                Background = true
            };
            
            //var queueIndexModeFirst = new CreateIndexModel<MessageQueue>(Builders<MessageQueue>.IndexKeys.Ascending(x => x.))
            
//            var queueIndexModelFirst = new CreateIndexModel<QueueMessage>(
//                Builders<QueueMessage>.IndexKeys
//                    .Ascending(x => x.ChatId), optionsUnique);
//
//            var queueIndexModelSecond = new CreateIndexModel<QueueMessage>(
//                Builders<QueueMessage>.IndexKeys
//                    .Ascending(x => x.IsHighPriority), optionsBackground);
            
//            queue.Indexes.CreateMany()

            return mongoDatabase;
        }
    }
}