using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnergiaMonitor.Models
{
    public class ConsumoEnergetico
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Permitir nulo para o MongoDB gerar automaticamente

        public DateTime DataHora { get; set; } = DateTime.UtcNow; // Data de registro
        public double ConsumoKwh { get; set; } // Consumo energético em kWh
        public string Local { get; set; } // Identificação do local
    }
}
