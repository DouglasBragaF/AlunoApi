using System;

public sealed class ResponsavelPersistence
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;

    public byte[] CpfCifrado { get; set; } = [];
    public byte[] CpfHash { get; set; } = [];

    public ICollection<AlunoResponsavelPersistence> Alunos { get; set; } = [];
}
