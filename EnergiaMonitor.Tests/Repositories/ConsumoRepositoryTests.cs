using EnergiaMonitor.Config;
using EnergiaMonitor.Models;
using EnergiaMonitor.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace EnergiaMonitor.Tests.Repositories
{
    public class ConsumoRepositoryTests
    {
        private readonly Mock<IMongoCollection<ConsumoEnergetico>> _mockCollection;
        private readonly ConsumoRepository _repository;

        public ConsumoRepositoryTests()
        {
            var mockDatabase = new Mock<IMongoDatabase>();
            _mockCollection = new Mock<IMongoCollection<ConsumoEnergetico>>();

            mockDatabase.Setup(db => db.GetCollection<ConsumoEnergetico>(It.IsAny<string>(), null))
                        .Returns(_mockCollection.Object);

            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
                      .Returns(mockDatabase.Object);

            var config = Options.Create(new MongoDbConfig
            {
                DatabaseName = "testdb",
                CollectionName = "testcollection"
            });

            _repository = new ConsumoRepository(config, mockClient.Object);
        }

        [Fact]
        public async Task InserirConsumo_DeveChamarInsertOneAsync()
        {
            // Arrange
            var consumo = new ConsumoEnergetico { ConsumoKwh = 50, Local = "Escritório" };

            // Act
            await _repository.InserirConsumo(consumo);

            // Assert
            _mockCollection.Verify(x => x.InsertOneAsync(consumo, null, default), Times.Once);
        }
    }
}
