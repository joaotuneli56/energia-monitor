using EnergiaMonitor.Models;
using EnergiaMonitor.Repositories;
using EnergiaMonitor.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnergiaMonitor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitorController : ControllerBase
    {
        private readonly ConsumoRepository _repository;
        private readonly CacheService _cacheService;

        public MonitorController(ConsumoRepository repository, CacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
        }

        [HttpPost("consumo")]
        public async Task<IActionResult> RegistrarConsumo([FromBody] ConsumoEnergetico consumo)
        {
            if (consumo == null || consumo.ConsumoKwh <= 0)
                return BadRequest("Dados de consumo inválidos.");

            await _repository.InserirConsumo(consumo);
            return Created("", consumo);
        }

        [HttpGet("consumo")]
        public async Task<IActionResult> ConsultarConsumos()
        {
            const string cacheKey = "consumos_cache";

            // Tenta obter do cache
            var consumosCache = await _cacheService.GetAsync<List<ConsumoEnergetico>>(cacheKey);
            if (consumosCache != null)
            {
                return Ok(new { source = "cache", data = consumosCache });
            }

            // Consulta no banco de dados
            var consumos = await _repository.ObterConsumos();
            if (consumos == null || consumos.Count == 0)
                return NotFound("Nenhum dado encontrado.");

            // Armazena no cache
            await _cacheService.SetAsync(cacheKey, consumos, TimeSpan.FromMinutes(5));

            return Ok(new { source = "database", data = consumos });
        }
    }
}
