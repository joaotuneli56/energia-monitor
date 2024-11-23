using EnergiaMonitor.Services;
using Moq;
using StackExchange.Redis;
using System.Text.Json;
using Xunit;

namespace EnergiaMonitor.Tests.Services
{
    public class CacheServiceTests
    {
        private readonly Mock<IDatabase> _mockDatabase;
        private readonly CacheService _cacheService;

        public CacheServiceTests()
        {
            var mockRedis = new Mock<IConnectionMultiplexer>();
            _mockDatabase = new Mock<IDatabase>();
            mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), null)).Returns(_mockDatabase.Object);

            _cacheService = new CacheService(mockRedis.Object);
        }

        [Fact]
        public async Task GetAsync_DeveRetornarValorDoCache()
        {
            // Arrange
            var key = "test_key";
            var expected = new List<string> { "valor1", "valor2" };
            var serialized = JsonSerializer.Serialize(expected);

            _mockDatabase.Setup(db => db.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(serialized);

            // Act
            var result = await _cacheService.GetAsync<List<string>>(key);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAsync_DeveRetornarNullQuandoRedisFalha()
        {
            // Arrange
            var key = "test_key";
            _mockDatabase.Setup(db => db.StringGetAsync(key, CommandFlags.None)).ThrowsAsync(new Exception("Erro no Redis"));

            // Act
            var result = await _cacheService.GetAsync<List<string>>(key);

            // Assert
            Assert.Null(result);
        }
    }
}
