namespace Gateway.DTOs;

/// <summary>
/// DTO для создания новой посылки.
/// </summary>
public record CreatePackageDto(
    long Weight,
    long Height,
    long Width,
    long Length
);