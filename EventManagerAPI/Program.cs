using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EventManagerAPI.Models;
using EventManagerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.Configure<MongoDbSettings>(
builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<EventService>();

// Add Swagger for API documentation (optional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Define API endpoints
var locations = app.MapGroup("/api/track");

// This is to test the connection.
app.MapGet("/", () => "Hello world");


app.Run();
