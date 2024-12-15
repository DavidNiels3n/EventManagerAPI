using EventManagerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventManagerAPI.Services
{
    public class EventService
    {
        private readonly IMongoCollection<Events> _eventsCollection;

        public EventService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _eventsCollection = mongoDatabase.GetCollection<Events>("Events"); // Collection name
        }

        public async Task<List<Events>> GetAllAsync() =>
            await _eventsCollection.Find(_ => true).ToListAsync();


        public async Task<List<Events>> GetApprovedEventsAsync() =>
            await _eventsCollection.Find(e => e.EventStatus == "Approved").ToListAsync();


        // Get event by ID
        public async Task<Events?> GetByIdAsync(string id) =>
            await _eventsCollection.Find(e => e.EventId == ObjectId.Parse(id).ToString()).FirstOrDefaultAsync();

        public async Task CreateAsync(Events newEvent) =>
            await _eventsCollection.InsertOneAsync(newEvent);

        // Update event by ID
        public async Task UpdateAsync(string id, Events updatedEvent) =>
            await _eventsCollection.ReplaceOneAsync(e => e.EventId == ObjectId.Parse(id).ToString(), updatedEvent);

        // Delete event by ID
        public async Task DeleteAsync(string id) =>
            await _eventsCollection.DeleteOneAsync(e => e.EventId == ObjectId.Parse(id).ToString());
    }
}
