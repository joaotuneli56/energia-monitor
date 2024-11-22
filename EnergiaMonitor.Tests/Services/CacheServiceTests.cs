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
            var key = "test_key";
            var expected = new List<string> { "value1", "value2" };
            var serialized = JsonSerializer.Serialize(expected);

            // Configura o comportamento do Redis para retornar o valor serializado
            _mockDatabase.Setup(db => db.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(serialized);

            // Chama o método a ser testado
            var result = await _cacheService.GetAsync<List<string>>(key);

            // Verifica o resultado
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task SetAsync_DeveArmazenarNoCache()
        {
            var key = "test_key";
            var value = new List<string> { "value1", "value2" };
            var expiration = TimeSpan.FromMinutes(5);
            var serialized = JsonSerializer.Serialize(value);

            // Configura o comportamento do Redis para armazenar o valor
            _mockDatabase.Setup(db => db.StringSetAsync(
                key,
                serialized,
                expiration,
                When.Always,              // Substitui null por When.Always
                CommandFlags.None)).ReturnsAsync(true);

            // Chama o método a ser testado
            await _cacheService.SetAsync(key, value, expiration);

            // Verifica se o método foi chamado corretamente
            _mockDatabase.Verify(db => db.StringSetAsync(
                key,
                It.IsAny<RedisValue>(),  // Use It.IsAny para evitar o problema com a expressão
                expiration,
                When.Always,              // Substitui null por When.Always
                CommandFlags.None),       // Substitui default por CommandFlags.None
                Times.Once);
        }
    }
}
