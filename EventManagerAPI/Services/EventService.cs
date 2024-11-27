using EventManagerAPI.Models;
using Microsoft.Extensions.Options;
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

        public async Task<Events?> GetByIdAsync(int id) =>
            await _eventsCollection.Find(e => e.EventId == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Events newEvent) =>
            await _eventsCollection.InsertOneAsync(newEvent);

        public async Task UpdateAsync(int id, Events updatedEvent) =>
            await _eventsCollection.ReplaceOneAsync(e => e.EventId == id, updatedEvent);

        public async Task DeleteAsync(int id) =>
            await _eventsCollection.DeleteOneAsync(e => e.EventId == id);
    }
}
