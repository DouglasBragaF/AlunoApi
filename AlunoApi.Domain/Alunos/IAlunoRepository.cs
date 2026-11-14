using System;

public interface IAlunoRepository
{
    Task<Aluno?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<int> ObterProximaSequenciaMatriculaAsync(CancellationToken ct = default);
    Task AdicionarAsync(Aluno aluno, CancellationToken ct = default);
}
