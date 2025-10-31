using Gateway.Contracts.DTOs;

namespace Gateway.Contracts;

public interface IOrdersApi
{
    Task<OrderDtoWithLink> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct);
    Task<OrderDtoWithLink?> GetOrderByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<OrderDtoWithLink>> GetAllOrdersAsync(CancellationToken ct);
    Task UpdateOrderStatusAsync(Guid id, UpdateOrderStatusDto dto, CancellationToken ct);
    Task DeleteOrderAsync(Guid id, CancellationToken ct);
}
