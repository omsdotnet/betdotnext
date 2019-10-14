using MongoDB.Bson;

namespace BetDotNext.Models
{
    public class User
    {
        public ObjectId UserId { get; set; }
        public string UserName { get; set; }
    }
}