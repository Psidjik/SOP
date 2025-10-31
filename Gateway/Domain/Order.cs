namespace Gateway.Domain;

/// <summary>
/// Заказ
/// </summary>
public class Order
{
    public Order(string email, string destinationAddress, List<Package> packages)
    {
        Id = Guid.NewGuid();
        Email = email;
        DestinationAddress = destinationAddress;
        CreatedAt = DateTime.UtcNow;
        Packages = packages;
        
        CalculateTotalCost();
    }

    public Order() { }

    /// <summary>
    /// Id заказа
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Email клиента
    /// </summary>
    public string Email { get; private  set; }

    /// <summary>
    /// Адрес доставки
    /// </summary>
    public string DestinationAddress { get; private set; }

    /// <summary>
    /// Дата создания заказа
    /// </summary>
    public DateTime CreatedAt { get; private set; }
    
    /// <summary>
    /// Список посылок в заказе
    /// </summary>
    public List<Package> Packages { get; private set; } = [];
    
    /// <summary>
    /// Статус заказа
    /// </summary>
    public OrderStatus Status { get; private set; } = OrderStatus.Created;
    
    /// <summary>
    /// Общая стоимость заказа
    /// </summary>
    public decimal TotalCost { get; private set; }
    
    /// <summary>
    /// Метод для смены статуса
    /// </summary>
    public void UpdateStatus(OrderStatus newStatus) => Status = newStatus;

    /// <summary>
    /// Пересчитывает общую стоимость доставки всех посылок.
    /// </summary>
    private void CalculateTotalCost() => TotalCost = Packages.Select(p => p.DeliveryCost).Sum();
}
