using Gateway.Contracts;
using Gateway.Contracts.DTOs;
using Gateway.Domain;
using Gateway.DomainData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Application;

public class OrdersApi(GatewayDbContext context, LinkGenerator linkGenerator, IHttpContextAccessor httpAccessor) : IOrdersApi
{
    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct)
    {
        if (dto.Packages == null || dto.Packages.Count == 0)
            throw new ArgumentException("Order must contain at least one package");

        var packages = dto.Packages
            .Select(p => new Package(p.Weight, p.Height, p.Width, p.Length, deliveryCost: 0m, orderId: Guid.Empty))
            .ToList();

        var order = new Order(dto.Email, dto.DestinationAddress, DateTime.UtcNow, packages);
        order.CalculateDeliveryCost();

        foreach (var p in order.Packages)
            p.OrderId = order.Id;

        context.Orders.Add(order);
        await context.SaveChangesAsync(ct);

        return BuildOrderDto(order);
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid id, CancellationToken ct)
    {
        var order = await context.Orders
            .Include(o => o.Packages)
            .FirstOrDefaultAsync(o => o.Id == id, ct)
             ?? throw new KeyNotFoundException();

        return BuildOrderDto(order);
    }

    public async Task<IReadOnlyList<OrderDto>> GetAllOrdersAsync(CancellationToken ct)
    {
        var orders = await context.Orders
            .Include(o => o.Packages)
            .ToListAsync(ct);

        return orders.Select(BuildOrderDto).ToList();
    }

    public async Task UpdateOrderStatusAsync(Guid id, UpdateOrderStatusDto status, CancellationToken ct)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id, ct)
                    ?? throw new KeyNotFoundException("Order not found");

        order.UpdateStatus(status.Status);

        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteOrderAsync(Guid id, CancellationToken ct = default)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id, ct)
                    ?? throw new KeyNotFoundException("Order not found");

        context.Orders.Remove(order);

        await context.SaveChangesAsync(ct);
    }
    
    
    private OrderDto BuildOrderDto(Order order)
    {
        var packageDtos = order.Packages
            .Select(p => new PackageDto(p.Id, p.Weight, p.Height, p.Width, p.Length, p.DeliveryCost))
            .ToList();

        var http = httpAccessor.HttpContext;
        
        string Self() => linkGenerator.GetUriByName(http!, "GetOrderById", new { id = order.Id });
        string Update() => linkGenerator.GetUriByName(http!, "UpdateOrder", new { id = order.Id });
        string UpdateStatus() => linkGenerator.GetUriByName(http, "UpdateOrderStatus", new { id = order.Id });
        string Delete() => linkGenerator.GetUriByName(http!, "DeleteOrder", new { id = order.Id });
        string All() => linkGenerator.GetUriByName(http, "GetAllOrders", null);

        var links = new List<LinkDto>
        {
            new("self", Self(), "GET"),
            new("update", Update(), "PUT"),
            new("update-status", UpdateStatus(), "PUT"),
            new("delete", Delete(), "DELETE"),
            new("all", All(), "GET")
        };

        return new OrderDto(
            order.Id,
            order.Email,
            order.DestinationAddress,
            order.CreatedAt,
            order.Status,
            order.TotalCost,
            packageDtos,
            links
        );
    }
}
