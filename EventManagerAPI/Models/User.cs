using MongoDB.Bson.Serialization.Attributes;

namespace EventManagerAPI.Models
{
    public class User
    {
        [BsonId]
        public int UserId { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
   
        public string UserPassword { get; set; } 

        public int EventCount { get; set; }



    }
}
