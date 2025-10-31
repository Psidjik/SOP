using Gateway.Contracts;
using Gateway.Contracts.DTOs;
using Gateway.Domain;
using Gateway.DomainData;
using Gateway.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Application;

public class OrdersApi(GatewayDbContext context,
    LinkGenerator linkGenerator,
    IHttpContextAccessor httpAccessor,
    ILogger<OrdersApi> logger)
    : IOrdersApi
{
    public async Task<OrderDtoWithLink> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct)
    {
        logger.LogInformation("Создание заказа для {Email}", dto.Email);

        EmailValidator.Validate(dto.Email);

        if (dto.Packages.Count == 0)
        {
            logger.LogWarning("Попытка создать заказ без посылок");
            throw new ArgumentException("Заказ должен содержать посылки");
        }

        var packages = dto.Packages
            .Select(p => new Package(p.Weight, p.Height, p.Width, p.Length, Guid.Empty))
            .ToList();

        var order = new Order(dto.Email, dto.DestinationAddress, packages);

        foreach (var p in order.Packages)
            p.OrderId = order.Id;

        context.Orders.Add(order);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Заказ {OrderId} успешно создан", order.Id);

        return BuildOrderDto(order);
    }

    public async Task<OrderDtoWithLink?> GetOrderByIdAsync(Guid id, CancellationToken ct)
    {
        logger.LogInformation("Получение заказа {OrderId}", id);

        var order = await context.Orders
            .Include(o => o.Packages)
            .FirstOrDefaultAsync(o => o.Id == id, ct)
            ?? throw new OrderNotFoundException(id);

        logger.LogInformation("Заказ {OrderId} найден", id);

        return BuildOrderDto(order);
    }

    public async Task<IReadOnlyList<OrderDtoWithLink>> GetAllOrdersAsync(CancellationToken ct)
    {
        logger.LogInformation("Получение всех заказов");

        var orders = await context.Orders
            .Include(o => o.Packages)
            .ToListAsync(ct);

        logger.LogInformation("Найдено {Count} заказов", orders.Count);

        return orders.Select(BuildOrderDto).ToList();
    }

    public async Task UpdateOrderStatusAsync(Guid id, UpdateOrderStatusDto status, CancellationToken ct)
    {
        logger.LogInformation("Обновление статуса заказа {OrderId} на {Status}", id, status.Status);

        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id, ct)
                    ?? throw new OrderNotFoundException(id);

        order.UpdateStatus(status.Status);

        await context.SaveChangesAsync(ct);

        logger.LogInformation("Статус заказа {OrderId} успешно обновлен", id);
    }

    public async Task DeleteOrderAsync(Guid id, CancellationToken ct = default)
    {
        logger.LogInformation("Удаление заказа {OrderId}", id);

        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id, ct)
                    ?? throw new OrderNotFoundException(id);

        context.Orders.Remove(order);
        await context.SaveChangesAsync(ct);

        logger.LogInformation("Заказ {OrderId} успешно удален", id);
    }

    private OrderDtoWithLink BuildOrderDto(Order order)
    {
        var packageDtos = order.Packages
            .Select(p => new PackageDto(p.Id, p.Weight, p.Height, p.Width, p.Length, p.DeliveryCost))
            .ToList();

        var http = httpAccessor.HttpContext;

        string Self() => linkGenerator.GetUriByName(http, "GetOrderById", new { id = order.Id });
        string Delete() => linkGenerator.GetUriByName(http, "DeleteOrder", new { id = order.Id });

        var links = new List<LinkDto>
        {
            new("self", Self(), "GET"),
            new("delete", Delete(), "DELETE"),
        };

        return new OrderDtoWithLink(
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
