using System;

public sealed record Cpf
{
    public string Valor { get; }
    private Cpf(string valor) => Valor = valor;

    public static Cpf Criar(string? valor)
    {
        var numeros = Normalizar(valor);
        Guard.Against(!EhValido(numeros), "CPF inválido.");
        return new Cpf(numeros);
    }

    public static bool EhValido(string? valor)   // reutilizável em validators
    {
        var c = Normalizar(valor);
        return c.Length == 11
            && c.Distinct().Count() > 1
            && c[9] - '0' == CalcularDigito(c, 9)
            && c[10] - '0' == CalcularDigito(c, 10);
    }

    public string Mascarado() => $"***.***.***-{Valor[^2..]}";
    public override string ToString() => Mascarado();

    private static string Normalizar(string? valor) =>
        string.Concat((valor ?? "").Where(char.IsAsciiDigit));

    private static int CalcularDigito(string c, int tamanho)
    {
        var soma = 0;
        for (var i = 0; i < tamanho; i++) soma += (c[i] - '0') * (tamanho + 1 - i);
        var resto = soma * 10 % 11;
        return resto == 10 ? 0 : resto;
    }
}
