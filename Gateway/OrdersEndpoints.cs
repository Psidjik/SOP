using Gateway.Contracts;
using Gateway.Contracts.DTOs;

namespace Gateway;

public static class OrdersEndpoints
{
    public static void MapOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/orders", async (CreateOrderDto dto, IOrdersApi service, HttpContext http) =>
        {
            var ct = http.RequestAborted; 
            var created = await service.CreateOrderAsync(dto, ct);

            var location = $"{http.Request.Scheme}://{http.Request.Host}{http.Request.PathBase}/api/orders/{created.Id}";
            return Results.Created(location, created);
        })
        .WithName("CreateOrder")
        .WithOpenApi();

        app.MapGet("/api/orders/{id:guid}", async (Guid id, IOrdersApi service, HttpContext http) =>
        {
            var ct = http.RequestAborted;
            var order = await service.GetOrderByIdAsync(id, ct);

            return Results.Ok(order);
        })
        .WithName("GetOrderById")
        .WithOpenApi();

        app.MapGet("/api/orders", async (IOrdersApi service, HttpContext http) =>
        {
            var ct = http.RequestAborted;
            var list = await service.GetAllOrdersAsync(ct);
            return Results.Ok(list);
        })
        .WithName("GetAllOrders")
        .WithOpenApi();

        app.MapPut("/api/orders/{id:guid}/status", async (Guid id, UpdateOrderStatusDto dto, IOrdersApi service, HttpContext http) =>
        {
            var ct = http.RequestAborted;
            await service.UpdateOrderStatusAsync(id, dto, ct);
            return Results.Ok(dto);
        })
        .WithName("UpdateOrderStatus")
        .WithOpenApi();

        app.MapDelete("/api/orders/{id:guid}", async (Guid id, IOrdersApi service, HttpContext http) =>
        {
            var ct = http.RequestAborted;
            await service.DeleteOrderAsync(id, ct);
            return Results.Ok(id);
        })
        .WithName("DeleteOrder")
        .WithOpenApi();
    }
}
