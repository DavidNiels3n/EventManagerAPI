﻿using EventManagerAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EventManagerAPI.Services
{
    public class UserService
    {

        private readonly IMongoCollection<User> _Userscollection;
        public UserService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _Userscollection = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task<List<User>> GetAllAsync() =>
            await _Userscollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetByIdAsync(int UserId) =>
            await _Userscollection.Find(e => e.UserId == UserId).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _Userscollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(int UserId, User updatedUser) =>
            await _Userscollection.ReplaceOneAsync(e => e.UserId == UserId, updatedUser);

        public async Task DeleteAsync(int UserId) =>
            await _Userscollection.DeleteOneAsync(e => e.UserId == UserId);

    }
}
