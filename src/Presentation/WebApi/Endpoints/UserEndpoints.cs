using Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("api/users").WithOpenApi()
                                            .WithTags("Users")
                                            .RequireAuthorization();

        root.MapGet("/", async ([FromServices] IMediator mediator, [FromQuery] int pageIndex, [FromQuery] int pageSize) => {
            return Results.Ok(await mediator.Send(new GetAllUsersQuery(pageIndex, pageSize)));
        });

        root.MapPost("/login", async (IMediator mediator, LoginUserQuery query) =>
        {
            var loginResult = await mediator.Send(query);
            return loginResult.IsSuccess ? Results.Ok(loginResult.Value) : Results.BadRequest(loginResult.Reasons);
        }).AllowAnonymous();

        root.MapPost("/register", async (IMediator mediator, RegisterUserCommand command) =>
        {
            var registerResult = await mediator.Send(command);

            return registerResult.IsSuccess ? Results.Created($"/{registerResult.Value}", new {id = registerResult.Value}) : Results.BadRequest(registerResult.Reasons);
        });

        root.MapGet("/{id}", async ([FromServices] IMediator mediator, Guid id) =>
        {
            var response = await mediator.Send(new GetUserByIdQuery(id));

            return response.IsSuccess ? Results.Ok(response.Value) : Results.NotFound(response.Reasons);
        });

        root.MapPut("/", async (IMediator mediator, UpdateUserCommand command) =>
        {
            var response = await mediator.Send(command);

            return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Reasons);
        });

        root.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
        {
            var response = await mediator.Send(new DeleteUserCommand(id));

            return response.IsSuccess ? Results.NoContent() : Results.BadRequest(response.Reasons);
        });
    }
}
