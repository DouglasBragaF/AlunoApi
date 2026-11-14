using System;

public sealed record Documento
{
    public Cpf Cpf { get; }
    public Rg? Rg { get; }
    public bool PossuiRg => Rg is not null;

    private Documento(Cpf cpf, Rg? rg) { Cpf = cpf; Rg = rg; }

    public static Documento Criar(Cpf cpf, Rg? rg = null)
    {
        ArgumentNullException.ThrowIfNull(cpf);
        return new Documento(cpf, rg);
    }

    public static Documento Criar(string? cpf, string? rg = null) =>
        new(Cpf.Criar(cpf), string.IsNullOrWhiteSpace(rg) ? null : Rg.Criar(rg));
}
