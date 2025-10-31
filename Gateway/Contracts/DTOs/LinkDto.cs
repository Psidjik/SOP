namespace Gateway.Contracts.DTOs;

/// <summary>
/// DTO для HATEOAS ссылки
/// </summary>
public class LinkDto
{
    /// <summary>
    /// Отношение (rel) ссылки — например "self", "update", "delete"
    /// </summary>
    public string Rel { get; set; }

    /// <summary>
    /// Полный URL, на который ссылается ссылка
    /// </summary>
    public string Href { get; set; }

    /// <summary>
    /// HTTP метод, который нужно использовать (GET, POST, PUT, DELETE)
    /// </summary>
    public string Method { get; set; }

    public LinkDto(string rel, string href, string method)
    {
        Rel = rel;
        Href = href;
        Method = method;
    }
}
