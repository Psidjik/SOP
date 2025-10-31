using Gateway.Contracts.DTOs;

namespace Gateway.Contracts;

public interface IOrdersApi
{
    Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct);
    Task<OrderDto?> GetOrderByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<OrderDto>> GetAllOrdersAsync(CancellationToken ct);
    Task UpdateOrderStatusAsync(Guid id, UpdateOrderStatusDto dto, CancellationToken ct);
    Task DeleteOrderAsync(Guid id, CancellationToken ct);
}
