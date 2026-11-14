using System;

public sealed class Responsavel : AggregateRoot
{
    public string Nome { get; private set; } = default!;
    public Cpf Cpf { get; private set; } = default!;

    private Responsavel() { } // EF Core

    public static Responsavel Criar(string nome, Cpf cpf)
    {
        ArgumentNullException.ThrowIfNull(cpf);
        return new Responsavel
        {
            Nome = Guard.AgainstNullOrWhiteSpace(nome, "Nome"),
            Cpf = cpf
        };
    }

    public void AtualizarDados(string nome, Cpf cpf)
    {
        ArgumentNullException.ThrowIfNull(cpf);
        Nome = Guard.AgainstNullOrWhiteSpace(nome, "Nome");
        Cpf = cpf;
    }
}
