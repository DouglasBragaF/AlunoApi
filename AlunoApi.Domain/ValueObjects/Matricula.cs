using System;

public sealed record Matricula
{
    public int Ano { get; }
    public int Sequencia { get; }
    public string Valor => $"{Ano}{Sequencia:D6}";

    private Matricula(int ano, int sequencia) { Ano = ano; Sequencia = sequencia; }

    public const int SequenciaMaxima = 999_999;

    public static Matricula Criar(int ano, int sequencia)
    {
        Guard.Against(ano is < 2000 or > 2999, "Ano da matrícula inválido.");
        Guard.Against(
            sequencia is <= 0 or > SequenciaMaxima,
            "Sequência da matrícula deve estar entre 1 e 999999.");

        return new Matricula(ano, sequencia);
    }

    public static Matricula Parse(string? valor)   // reconstrói a partir de texto (ex.: "2026000123")
    {
        if (valor is not { Length: 10 } || !valor.All(char.IsAsciiDigit))
            throw new DomainException("Matrícula inválida.");
        return Criar(int.Parse(valor[..4]), int.Parse(valor[4..]));
    }

    public override string ToString() => Valor;
}
