namespace EventManagerAPI.Endpoints;
using EventManagerAPI.Models;
using EventManagerAPI.Services;

    public static class EventsApi
    {

         public static IEndpointRouteBuilder MapEventsEndpoints(this IEndpointRouteBuilder app){

         
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

        return app; 
    }
}
    
        
