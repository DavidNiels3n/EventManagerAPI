using MongoDB.Bson.Serialization.Attributes;

namespace EventManagerAPI.Models
{
    public class Events
    {
        [BsonId]

        public int EventId { get; set; }

        public string? EventName { get; set; }
        public string? EventDescription { get; set; }

        public string? EventCategory { get; set; }

        public string? EventStart { get; set; }

        public string? EventEnd { get; set; }
        public string? EventStatus { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EventCreationTimestamp { get; set; } = DateTime.Now;
    }
}
