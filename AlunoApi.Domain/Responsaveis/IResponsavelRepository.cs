using System;

public interface IResponsavelRepository
{
    Task<Responsavel?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task AdicionarAsync(Responsavel responsavel, CancellationToken ct = default);
    Task<bool> ExisteCpfAsync(Cpf cpf, CancellationToken ct = default);
}
