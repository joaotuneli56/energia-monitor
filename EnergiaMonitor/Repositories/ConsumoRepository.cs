using EnergiaMonitor.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EnergiaMonitor.Config;

namespace EnergiaMonitor.Repositories
{
    public class ConsumoRepository
    {
        private readonly IMongoCollection<ConsumoEnergetico> _collection;

        public ConsumoRepository(IOptions<MongoDbConfig> config, IMongoClient client)
        {
            var database = client.GetDatabase(config.Value.DatabaseName);
            _collection = database.GetCollection<ConsumoEnergetico>(config.Value.CollectionName);
        }

        public async Task InserirConsumo(ConsumoEnergetico consumo) => await _collection.InsertOneAsync(consumo);

        public async Task<List<ConsumoEnergetico>> ObterConsumos() => await _collection.Find(_ => true).ToListAsync();
    }
}
