namespace Gateway.Domain;

/// <summary>
/// Посылка
/// </summary>
public class Package
{
    public Package(long weight, long height, long width, long length, Guid orderId)
    {
        Id = Guid.NewGuid();
        Weight = weight;
        Height = height;
        Width = width;
        Length = length;
        OrderId = orderId;
        
        CalculateDeliveryCost();
    }

    private Package(){}
    
    /// <summary>
    /// Id посылки
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Вес посылки.
    /// </summary>
    public long Weight { get; set; }

    /// <summary>
    /// Высота посылки в см
    /// </summary>
    public long Height { get; set; }

    /// <summary>
    /// Ширина посылки в см
    /// </summary>
    public long Width { get; set; }

    /// <summary>
    /// Длина посылки в см
    /// </summary>
    public long Length { get; set; }

    /// <summary>
    /// Стоимость доставки для этой посылки
    /// </summary>
    public decimal DeliveryCost { get; set; }

    /// <summary>
    /// Id заказа
    /// </summary>
    public Guid OrderId { get; set; }
    
    /// <summary>
    /// Подсчет стоимости
    /// </summary>
    private void CalculateDeliveryCost() => DeliveryCost = Weight * 1m + (Height * Width * Length) * 0.01m;
}