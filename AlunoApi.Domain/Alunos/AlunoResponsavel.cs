using System;

public sealed class AlunoResponsavel
{
    public Guid AlunoId { get; private set; }
    public Guid ResponsavelId { get; private set; }

    private AlunoResponsavel() { } // EF Core
    internal AlunoResponsavel(Guid alunoId, Guid responsavelId)
    {
        AlunoId = alunoId;
        ResponsavelId = responsavelId;
    }
}
