﻿using Application;
using MediatR;

namespace WebApi;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this WebApplication webApplication)
    {
        var root = webApplication.MapGroup("api/products").WithOpenApi()
                                                            .WithTags("Products")
                                                            .RequireAuthorization();

        root.MapGet("/", async (IMediator mediator, int pageIndex, int pageSize) =>
        {
            return Results.Ok(await mediator.Send(new GetAllProductsQuery(pageIndex, pageSize)));
        });

        root.MapGet("/{id}", async (IMediator mediator, Guid id) =>
        {
            var response = await mediator.Send(new GetProductByIdQuery(id));

            return response.IsSuccess ? Results.Ok(response.Value) : Results.NotFound(response.Errors);
        });

        root.MapPost("/", async (IMediator mediator, CreateProductCommand command) =>
        {
            var createResult = await mediator.Send(command);

            return createResult.IsSuccess ? Results.Created($"/{createResult.Value.Id}", new {id = createResult.Value.Id}) : Results.BadRequest(createResult.Reasons);
        });

        root.MapPut("/", async (IMediator mediator, UpdateProductCommand command) =>
        {
            var updateResult = await mediator.Send(command);

            return updateResult.IsSuccess ? Results.Ok(updateResult.Value) : Results.BadRequest(updateResult.Reasons);
        });

        root.MapDelete("/{id}", async (IMediator mediator, Guid id) =>
        {
            var deleteResult = await mediator.Send(new DeleteProductCommand(id));

            return deleteResult.IsSuccess ? Results.NoContent() : Results.BadRequest(deleteResult.Reasons);
        });
    }
}
