﻿using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints;

//- Accepts the order ID as a parameter.
//- Constructs a DeleteOrderCommand.
//- Sends the command using MediatR.
//- Returns a success or not found response.

//public record DeleteOrderRequest(Guid OrderId);
public record DeleteOrderResponse(bool IsSuccess);

public class DeleteOrder :ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteOrderCommand(id);
            
            var result = await sender.Send(command);
            
            var response = result.Adapt<DeleteOrderResponse>();
            
            return Results.Ok(response);
        })
        .WithName("DeleteOrder")
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Order")
        .WithDescription("Delete Order");
    }
}