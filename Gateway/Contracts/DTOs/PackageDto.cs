namespace Gateway.Contracts.DTOs;

/// <summary>
/// DTO для отображения посылки.
/// </summary>
public record PackageDto(
    Guid Id,
    long Weight,
    long Height,
    long Width,
    long Length,
    decimal DeliveryCost
);