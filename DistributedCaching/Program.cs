using DistributedCaching.Extensions;
using DistributedCaching.Services;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adds Shared services to get post data
builder.Services.AddServices();

// Adds Distributed Memory Cache
// builder.Services.AddDistributedMemoryCache();

// Adds Distributed NCache Cache
// builder.Services.AddNCacheDistributedCache(options =>
// {
//     options.CacheName = builder.Configuration["NCache"];
//     options.EnableLogs = true;
//     options.ExceptionsEnabled = true;
// });

// Adds Distributed Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:Configuration"];
    options.InstanceName = builder.Configuration["Redis:Instance"];
});

// StackExchange.Redis API Service
builder.Services.AddSingleton<RedisService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Extension method to connect to Redis
app.UseRedis();

app.Run();
