using SantanderTest.Contract.Entities;
using SantanderTest.Contract.Infrastructure;
using SantanderTest.Contract.Services;
using SantanderTest.Middleware;
using SantanderTest.Service.Infrastructure;
using SantanderTest.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
// Register the registry service
builder.Services.AddScoped<INewsService, HackerNewsService>();
// Cache service is genereic but for current taks I'm resolving with specific types
builder.Services.AddSingleton<ICacheService<int, Story>, ConcurrentDictionaryCacheService<int, Story>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();

