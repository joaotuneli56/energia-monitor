using EnergiaMonitor.Config;
using EnergiaMonitor.Repositories;
using EnergiaMonitor.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Configuração do MongoDB sem autenticação
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDbConfig"));
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var config = sp.GetRequiredService<IOptions<MongoDbConfig>>().Value;
    return new MongoClient(config.ConnectionString);
});
builder.Services.AddScoped<ConsumoRepository>();

// Configuração do Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(
    builder.Configuration.GetSection("RedisConfig:ConnectionString").Value ?? "localhost:6379"));
builder.Services.AddSingleton<CacheService>();

// Configuração dos serviços do ASP.NET
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração para ambientes de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
