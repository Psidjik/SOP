using Gateway.Domain;

namespace Gateway.Contracts.DTOs;

public record OrderDtoWithLink(
    Guid Id,
    string Email,
    string DestinationAddress,
    DateTime CreatedAt,
    OrderStatus Status,
    decimal TotalCost,
    List<PackageDto> Packages,
    List<LinkDto> Links);