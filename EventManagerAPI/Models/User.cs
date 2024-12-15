using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventManagerAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; } 

        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPassword { get; set; }
        public string Role { get; set; } = "User"; // Default value for new users

        //Set to 0 because nothing is passed from frontend
        public int EventCount { get; set; } = 0;



    }
}