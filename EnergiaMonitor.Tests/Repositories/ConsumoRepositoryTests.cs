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
            var mockDb = new Mock<IMongoDatabase>();
            _mockCollection = new Mock<IMongoCollection<ConsumoEnergetico>>();
            mockDb.Setup(db => db.GetCollection<ConsumoEnergetico>(It.IsAny<string>(), null))
                  .Returns(_mockCollection.Object);

            var mockClient = new Mock<IMongoClient>();
            mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
                      .Returns(mockDb.Object);

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
            var consumo = new ConsumoEnergetico { ConsumoKwh = 100, Local = "Test" };

            await _repository.InserirConsumo(consumo);

            _mockCollection.Verify(x => x.InsertOneAsync(consumo, null, default), Times.Once);
        }

        [Fact]
        public async Task ObterConsumos_DeveRetornarLista()
        {
            var mockCursor = new Mock<IAsyncCursor<ConsumoEnergetico>>();
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                      .Returns(true)
                      .Returns(false);
            mockCursor.Setup(c => c.Current)
                      .Returns(new List<ConsumoEnergetico>
                      {
                          new ConsumoEnergetico { ConsumoKwh = 100, Local = "Test" }
                      });

            _mockCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<ConsumoEnergetico>>(),
                                                   null, default))
                           .ReturnsAsync(mockCursor.Object);

            var result = await _repository.ObterConsumos();

            Assert.Single(result);
            Assert.Equal(100, result[0].ConsumoKwh);
        }
    }
}
