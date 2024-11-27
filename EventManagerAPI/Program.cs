using Microsoft.Extensions.Options;
using MongoDB.Driver;
using EventManagerAPI.Models;
using EventManagerAPI.Services;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add MongoDB settings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB")
);

// Register services for dependency injection
builder.Services.AddSingleton<EventService>();
builder.Services.AddSingleton<UserService>(); // Assuming you add a UserService for user management

// Enable Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//@@@@@@@@@@@@@@@@@@@@@@@@@@@@::EVENTS::@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
var eventsApi = app.MapGroup("/api/events");
eventsApi.MapGet("/", async (EventService eventService) =>
    await eventService.GetAllAsync())
    .WithName("GetAllEvents");

eventsApi.MapGet("/{id}", async (int id, EventService eventService) =>
{
    var eventItem = await eventService.GetByIdAsync(id);
    return eventItem is not null ? Results.Ok(eventItem) : Results.NotFound();
})
.WithName("GetEventById");

eventsApi.MapPost("/", async (Events newEvent, EventService eventService) =>
{
    await eventService.CreateAsync(newEvent);
    return Results.Created($"/api/events/{newEvent.EventId}", newEvent);
})
.WithName("CreateEvent");

eventsApi.MapPut("/{id}", async (int id, Events updatedEvent, EventService eventService) =>
{
    var existingEvent = await eventService.GetByIdAsync(id);
    if (existingEvent is null)
    {
        return Results.NotFound();
    }
    await eventService.UpdateAsync(id, updatedEvent);
    return Results.NoContent();
})
.WithName("UpdateEvent");

eventsApi.MapDelete("/{id}", async (int id, EventService eventService) =>
{
    var existingEvent = await eventService.GetByIdAsync(id);
    if (existingEvent is null)
    {
        return Results.NotFound();
    }
    await eventService.DeleteAsync(id);
    return Results.NoContent();
})
.WithName("DeleteEvent");


//@@@@@@@@@@@@@@@@@@@@@@@@@@@@::USERS::@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

var usersApi = app.MapGroup("/api/users");

usersApi.MapGet("/", async (UserService userService) =>
    await userService.GetAllAsync())
    .WithName("GetAllUsers");

usersApi.MapGet("/{id}", async (int Userid, UserService userService) =>
{
    var userItem = await userService.GetByIdAsync(Userid);
    return userItem is not null ? Results.Ok(userItem) : Results.NotFound();
})
.WithName("GetUserById");

usersApi.MapPost("/", async (User newUser, UserService userService) =>
{
    await userService.CreateAsync(newUser);
    return Results.Created($"/api/events/{newUser.UserId}", newUser);
})
.WithName("CreateUser");



//@@@@@@@@@@@@@@@@@@@@@@@@@@@@::TEST::@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
// Add a simple route for health check or testing
app.MapGet("/", () => "Event Manager API is running!");

app.Run();
