namespace Gateway.DTOs;

/// <summary>
/// DTO для создания заказа.
/// </summary>
public record CreateOrderDto(
    string Email,
    string DestinationAddress,
    List<CreatePackageDto> Packages
);