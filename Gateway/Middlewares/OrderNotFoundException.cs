namespace Gateway.Middlewares;

/// <summary>
/// Исключение выбрасывается, если заказ с заданным Id не найден.
/// </summary>
public class OrderNotFoundException(Guid orderId) : Exception($"Заказ с Id '{orderId}' не найден.");