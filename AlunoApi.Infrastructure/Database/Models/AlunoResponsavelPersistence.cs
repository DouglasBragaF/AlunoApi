using System;

public sealed class AlunoResponsavelPersistence
{
    public Guid AlunoId { get; set; }
    public Guid ResponsavelId { get; set; }

    public AlunoPersistence Aluno { get; set; } = null!;
    public ResponsavelPersistence Responsavel { get; set; } = null!;
}