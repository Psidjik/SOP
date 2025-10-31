using System.Text.RegularExpressions;

namespace Gateway.Middlewares;

public static class EmailValidator
{
    // RFC 5322 базовый вариант регулярки
    private static readonly Regex EmailRegex = new(
        @"^(?("")(""[^""]+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'*+/=?^`{}|~\w])*)(?<=[0-9a-zA-Z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-0-9a-zA-Z]*[0-9a-zA-Z]*\.)+[a-zA-Z]{2,}))$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public static void Validate(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
            throw new InvalidEmailException(email);
    }
}