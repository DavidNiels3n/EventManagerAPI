namespace EventManagerAPI.Endpoints;
using EventManagerAPI.Models;
using EventManagerAPI.Services;
using Microsoft.AspNetCore.Mvc.Diagnostics;

    public static class EventsApi
    {

         public static IEndpointRouteBuilder MapEventsEndpoints(this IEndpointRouteBuilder app){

         
            var eventsApi = app.MapGroup("/api/events");
            eventsApi.MapGet("/", async (EventService eventService) =>
             await eventService.GetAllAsync())
            .WithName("GetAllEvents");


        //Get by ID
        eventsApi.MapGet("/{id}", async (string id, EventService eventService) =>
        {
            var eventItem = await eventService.GetByIdAsync(id);
            return eventItem is not null ? Results.Ok(eventItem) : Results.NotFound();
        })
        .WithName("GetEventById");


        //Approved
        eventsApi.MapGet("/approved", async (EventService eventService) =>
        {
            var approvedEvents = await eventService.GetApprovedEventsAsync();
            return Results.Ok(approvedEvents);
        })
        .WithName("GetApprovedEvents");



        //Create new event
        eventsApi.MapPost("/", async (Events newEvent, EventService eventService, UserService userService) =>
        {
            // Validate required fields using LINQ
            var requiredFields = new[] { newEvent.EventName, newEvent.EventStart, newEvent.EventEnd, newEvent.UserId, newEvent.EventLocation };
            if (requiredFields.Any(string.IsNullOrEmpty))
                return Results.BadRequest(new { message = "Missing required fields." });

            // Fetch user for eventstatus
            var user = await userService.GetByIdAsync(newEvent.UserId);
            if (user == null)
                return Results.NotFound(new { message = "User not found." });

            // Set status based on user role
            newEvent.EventStatus = (user.Role == "EventHolder" || user.Role == "Admin") ? "Approved" : "Pending";
            newEvent.EventCreationTimestamp = DateTime.Now;

            // Create event
            try
            {
                await eventService.CreateAsync(newEvent);
            }
            catch
            {
                return Results.Problem("Error saving event.");
            }

            return Results.Created($"/api/events/{newEvent.EventId}", newEvent);
            })
            .WithName("CreateEvent");



        //Update
        eventsApi.MapPut("/{id}", async (string id, Events updatedEvent, EventService eventService) =>
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


        //Delete
        eventsApi.MapDelete("/{id}", async (string id, EventService eventService) =>
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
    
        
