namespace Gateway.Domain;

public enum OrderStatus
{
    /// <summary>
    /// Заказ создан, но ещё не обработан
    /// </summary>
    Created = 0,
    
    /// <summary>
    /// Заказ в обработке
    /// </summary>
    Processing = 1,  
    
    /// <summary>
    /// Заказ отправлен
    /// </summary>
    Shipped = 2,
    
    /// <summary>
    /// Доставлен
    /// </summary>
    Delivered = 3,     
    
    /// <summary>
    /// Отменён
    /// </summary>
    Cancelled = 4
}