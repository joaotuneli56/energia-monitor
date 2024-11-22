namespace EnergiaMonitor.Config
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; } = "mongodb://localhost:27017"; // URL do MongoDB
        public string DatabaseName { get; set; } = "MonitorEnergiaDb";             // Nome do banco de dados
        public string CollectionName { get; set; } = "Consumos";                   // Nome da coleção
    }
}
