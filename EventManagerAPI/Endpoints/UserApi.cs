namespace EventManagerAPI.Endpoints;
using EventManagerAPI.Services;
using EventManagerAPI.Models;
using Microsoft.AspNetCore.Identity.Data;

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

            // Login
            usersApi.MapPost("/login", async (LoginRequest loginRequest, UserService userService) =>
            {
            // Attempt to find the user by their email
            var user = await userService.GetUserByEmailAsync(loginRequest.Email);
      
            //Return unauthorized if user doesnt exist in db
            if (user == null || user.UserPassword != loginRequest.Password)
            {
                return Results.Json(new { message = "Invalid email or password" }, statusCode: 401);
            }

            // If login is successful, return a success message with the user's ID
            return Results.Json(new { message = "Login successful", userId = user.UserId }, statusCode: 200);
            })
            .WithName("LoginUser");



        //Create user
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

