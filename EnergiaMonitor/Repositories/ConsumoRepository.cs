using EnergiaMonitor.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EnergiaMonitor.Config;
using MongoDB.Bson;

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

        public async Task InserirConsumo(ConsumoEnergetico consumo)
        {
            // Gera um novo ID se não for fornecido
            if (string.IsNullOrEmpty(consumo.Id))
            {
                consumo.Id = ObjectId.GenerateNewId().ToString();
            }

            var existente = await _collection.Find(c => c.Id == consumo.Id).FirstOrDefaultAsync();
            if (existente != null)
            {
                throw new Exception($"O documento com ID '{consumo.Id}' já existe.");
            }

            await _collection.InsertOneAsync(consumo);
        }

        public async Task<List<ConsumoEnergetico>> ObterConsumos()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}
