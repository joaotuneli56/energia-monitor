using StackExchange.Redis;
using System.Text.Json;

namespace EnergiaMonitor.Services
{
    public class CacheService
    {
        private readonly IDatabase _database;

        public CacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var json = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, json, expiration);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var json = await _database.StringGetAsync(key);
            return json.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(json);
        }
    }
}
