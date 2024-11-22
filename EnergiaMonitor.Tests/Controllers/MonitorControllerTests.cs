using EnergiaMonitor.Controllers;
using EnergiaMonitor.Models;
using EnergiaMonitor.Repositories;
using EnergiaMonitor.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EnergiaMonitor.Tests.Controllers
{
    public class MonitorControllerTests
    {
        private readonly Mock<ConsumoRepository> _mockRepository;
        private readonly Mock<CacheService> _mockCacheService;
        private readonly MonitorController _controller;

        public MonitorControllerTests()
        {
            _mockRepository = new Mock<ConsumoRepository>();
            _mockCacheService = new Mock<CacheService>();
            _controller = new MonitorController(_mockRepository.Object, _mockCacheService.Object);
        }

        [Fact]
        public async Task RegistrarConsumo_DeveRetornarCreated()
        {
            var consumo = new ConsumoEnergetico { ConsumoKwh = 100, Local = "Test" };

            var result = await _controller.RegistrarConsumo(consumo) as CreatedResult;

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task ConsultarConsumos_DeveRetornarDoCache()
        {
            var cacheData = new List<ConsumoEnergetico>
            {
                new ConsumoEnergetico { ConsumoKwh = 100, Local = "Cached" }
            };

            _mockCacheService.Setup(c => c.GetAsync<List<ConsumoEnergetico>>("consumos_cache"))
                             .ReturnsAsync(cacheData);

            var result = await _controller.ConsultarConsumos() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as dynamic;
            Assert.Equal("cache", response.source);
            Assert.Single(response.data);
        }

        [Fact]
        public async Task ConsultarConsumos_DeveRetornarDoBanco()
        {
            _mockCacheService.Setup(c => c.GetAsync<List<ConsumoEnergetico>>("consumos_cache"))
                             .ReturnsAsync((List<ConsumoEnergetico>)null);

            var dbData = new List<ConsumoEnergetico>
            {
                new ConsumoEnergetico { ConsumoKwh = 100, Local = "Database" }
            };

            _mockRepository.Setup(r => r.ObterConsumos()).ReturnsAsync(dbData);

            var result = await _controller.ConsultarConsumos() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as dynamic;
            Assert.Equal("database", response.source);
            Assert.Single(response.data);
        }
    }
}
