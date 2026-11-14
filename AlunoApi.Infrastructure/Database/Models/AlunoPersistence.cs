using System;

public sealed class AlunoPersistence
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public DateOnly DataNascimento { get; set; }

    public byte[] CpfCifrado { get; set; } = [];
    public byte[] CpfHash { get; set; } = [];

    public string? Rg { get; set; }

    public short MatriculaAno { get; set; }
    public int MatriculaSequencia { get; set; }
    public string Matricula { get; set; } = null!;

    public short Status { get; set; }

    public ICollection<AlunoResponsavelPersistence> Responsaveis { get; set; } = [];
}
