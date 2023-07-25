using RESTful_API.Core.Services.Implimentations;
using RESTful_API.Core.Services.Interfaces;
using RESTful_API.Data;
using RESTful_API.Data.Repositories.Implimentations;
using RESTful_API.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISupplyRepository, SupplyRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICategoryReposetory, CategoryReposetory>();
builder.Services.AddScoped<IOrderSupplyRepository, OrderSupplyRepository>();
builder.Services.AddScoped<IOrderSubscribeRepository, OrderSubscribeRepository>();
builder.Services.AddScoped<IBotService, BotService>();

var app = builder.Build();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
