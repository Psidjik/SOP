using Gateway.Domain;

namespace Gateway.DTOs;

/// <summary>
/// DTO для отображения заказа.
/// </summary>
public record OrderDto(
    Guid Id,
    string Email,
    string DestinationAddress,
    DateTime CreatedAt,
    OrderStatus Status,
    decimal TotalCost,
    List<PackageDto> Packages
);