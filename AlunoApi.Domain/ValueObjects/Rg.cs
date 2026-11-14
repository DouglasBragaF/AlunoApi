using System;

public sealed record Rg
{
    public string Valor { get; }
    private Rg(string valor) => Valor = valor;

    public static Rg Criar(string? valor)
    {
        var limpo = string.Concat((valor ?? "").Where(char.IsAsciiLetterOrDigit)).ToUpperInvariant();
        Guard.Against(limpo.Length is < 5 or > 14, "RG inválido.");
        return new Rg(limpo);
    }

    public string Mascarado() => new string('*', Valor.Length - 2) + Valor[^2..];
    public override string ToString() => Mascarado();
}
