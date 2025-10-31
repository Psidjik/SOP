using Gateway.Domain;

namespace Gateway.Contracts.DTOs;

/// <summary>
/// DTO для обновления статуса заказа.
/// </summary>
public record UpdateOrderStatusDto(
    OrderStatus Status
);