using System;
using BetDotNext.Models;
using MongoDB.Driver;

namespace BetDotNext.Setup
{
    public static class MongoDatabaseSetup
    {
        private const string QueueMessage = "QueueMessage";
        private const string Users = "Users";
        
        public static IMongoDatabase MongoDbInit(this IMongoDatabase mongoDatabase)
        {
            var queue = mongoDatabase.GetCollection<MessageQueue>(QueueMessage);
            var users = mongoDatabase.GetCollection<User>(Users);
            
            var optionsUnique = new CreateIndexOptions
            {
                Unique = true,
                Background = true
            };

            var optionsBackground = new CreateIndexOptions
            {
                Background = true,
            };

            try
            {
                var userIndexModel = new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Ascending(x => x.UserId), optionsUnique);
                
                var queueIdIndexModel = new CreateIndexModel<MessageQueue>(
                    Builders<MessageQueue>.IndexKeys.Ascending(x => x.Id), optionsUnique);
                
                var queueStartTimeIndex = new CreateIndexModel<MessageQueue>(
                    Builders<MessageQueue>.IndexKeys.Ascending(x => x.StartTime), optionsBackground);
                
                users.Indexes.CreateOne(userIndexModel);
                queue.Indexes.CreateMany(new[] { queueIdIndexModel, queueStartTimeIndex });
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message, ex);
            }

            return mongoDatabase;
        }
    }
}