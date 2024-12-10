namespace EventManagerAPI.Endpoints;
using EventManagerAPI.Services;
using EventManagerAPI.Models;

    public static class UserApi
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {

            var usersApi = app.MapGroup("/api/users");

            usersApi.MapGet("/", async (UserService userService) =>
                await userService.GetAllAsync())
                .WithName("GetAllUsers");

            usersApi.MapGet("/{userId}", async (string Userid, UserService userService) =>
            {
                var userItem = await userService.GetByIdAsync(Userid);
                return userItem is not null ? Results.Ok(userItem) : Results.NotFound();
            })
            .WithName("GetUserById");



            //testing
            usersApi.MapPost("/", async (User newUser, UserService userService) =>
            {
            Console.WriteLine($"Received Data: {System.Text.Json.JsonSerializer.Serialize(newUser)}");
            await userService.CreateAsync(newUser);
            return Results.Created($"/api/users/{newUser.UserId}", newUser);
            })
            .WithName("CreateUser");



        usersApi.MapPut("/{userId}", async (string userId, User updateUser, UserService userService) =>
            {
                var existingUser = await userService.GetByIdAsync(userId);
                if (existingUser is null)
                {
                    return Results.NotFound();
                }
                await userService.UpdateAsync(userId, updateUser);
                return Results.NoContent();

            })
                .WithName("UpdatedUser");

            usersApi.MapDelete("/{userId}", async (string userId, UserService userService) =>
            {
                var existingEvent = await userService.GetByIdAsync(userId);
                if (existingEvent is null)
                {
                    return Results.NotFound();
                }
                await userService.DeleteAsync(userId);
                return Results.NoContent();
            })
            .WithName("DeleteUser");

        return app; 



        }
    }

