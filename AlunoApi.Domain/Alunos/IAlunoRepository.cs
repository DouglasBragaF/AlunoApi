using System;

public interface IAlunoRepository
{
    Task<Aluno?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<int> ObterProximaSequenciaMatriculaAsync(int ano, CancellationToken ct = default);
    Task AdicionarAsync(Aluno aluno, CancellationToken ct = default);
}
