using System;

public static class Guard
{
    public static void Against(bool condition, string message)
    {
        if (condition) throw new DomainException(message);
    }

    public static string AgainstNullOrWhiteSpace(string? value, string field, int maxLength = 150)
    {
        Against(string.IsNullOrWhiteSpace(value), $"{field} é obrigatório.");
        var trimmed = value!.Trim();
        Against(trimmed.Length > maxLength, $"{field} deve ter no máximo {maxLength} caracteres.");
        return trimmed;
    }
}
