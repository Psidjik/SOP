namespace Gateway.Middlewares;

/// <summary>
/// Ошибка валидации Email
/// </summary>
public class InvalidEmailException(string email) : Exception($"Некорректный Email: {email}");