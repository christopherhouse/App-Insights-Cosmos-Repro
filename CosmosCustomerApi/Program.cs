using CosmosCustomerApi.Services;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add Application Insights telemetry
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Cosmos DB
var cosmosEndpointUri = builder.Configuration["CosmosDb:EndpointUri"];
var cosmosPrimaryKey = builder.Configuration["CosmosDb:PrimaryKey"];
var databaseName = builder.Configuration["CosmosDb:DatabaseName"];
var containerName = builder.Configuration["CosmosDb:ContainerName"];

if (!string.IsNullOrEmpty(cosmosEndpointUri) && 
    !string.IsNullOrEmpty(cosmosPrimaryKey) &&
    !string.IsNullOrEmpty(databaseName) &&
    !string.IsNullOrEmpty(containerName))
{
    builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
    {
        return new CosmosClient(cosmosEndpointUri, cosmosPrimaryKey);
    });

    builder.Services.AddSingleton<ICosmosDbService>(serviceProvider =>
    {
        var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();
        return new CosmosDbService(cosmosClient, databaseName, containerName);
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
