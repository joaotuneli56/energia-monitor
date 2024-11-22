namespace EnergiaMonitor.Models
{
    public class ConsumoEnergetico
    {
        public string Id { get; set; } // MongoDB ObjectId
        public DateTime DataHora { get; set; } = DateTime.UtcNow; // Data de registro
        public double ConsumoKwh { get; set; } // Consumo energético em kWh
        public string Local { get; set; } // Identificação do local
    }
}
