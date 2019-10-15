using System;
using BetDotNext.Models;
using MongoDB.Driver;

namespace BetDotNext.Data
{
    public class UserRepository
    {
        private readonly IMongoDatabase _mongoDatabase;
        
        public UserRepository(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase ?? throw new ArgumentNullException(nameof(mongoDatabase));
        }

        public User GetUserByChatId(long id) => null;
    }
}