using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EventManagerAPI.Models;
using EventManagerAPI.Services;
using Microsoft.Extensions.Logging;
using EventManagerAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add MongoDB settings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB")
);

// Register services for dependency injection
builder.Services.AddSingleton<EventService>();
builder.Services.AddSingleton<UserService>(); 

// Enable Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Enable Swagger UI in development
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection(); 

app.MapUserEndpoints();
app.MapEventsEndpoints(); 


//@@@@@@@@@@@@@@@@@@@@@@@@@@@@::TEST::@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
// Add a simple route for health check or testing
app.MapGet("/", () => "Event Manager API is running!");

app.Run();
