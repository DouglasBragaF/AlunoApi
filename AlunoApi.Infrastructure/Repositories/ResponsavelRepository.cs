using AlunoApi.Application.Security;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AlunoApi.Infrastructure.Repositories;

public sealed class ResponsavelRepository : IResponsavelRepository
{
    private readonly AppDbContext _context;
    private readonly ICpfProtector _cpfProtector;

    public ResponsavelRepository(
        AppDbContext context,
        ICpfProtector cpfProtector)
    {
        _context = context;
        _cpfProtector = cpfProtector;
    }

    public async Task<Responsavel?> ObterPorIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var registro = await _context.Responsaveis
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        return registro is null ? null : ParaDominio(registro);
    }

    public async Task AdicionarAsync(
    Responsavel responsavel,
    CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(responsavel);

        var cpfProtegido = _cpfProtector.Proteger(responsavel.Cpf);

        if (await CpfHashExisteAsync(cpfProtegido.Hash, ct))
            throw new DomainException(
                "Já existe um responsável cadastrado com este CPF.");

        var registro = new ResponsavelPersistence
        {
            Id = responsavel.Id,
            Nome = responsavel.Nome,
            CpfCifrado = cpfProtegido.Cifrado,
            CpfHash = cpfProtegido.Hash
        };

        _context.Responsaveis.Add(registro);

        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (
            ViolacaoDeUnicidadeCpf(ex, "uq_responsaveis_cpf_hash"))
        {
            throw new DomainException(
                "Já existe um responsável cadastrado com este CPF.");
        }
    }

    private Responsavel ParaDominio(ResponsavelPersistence registro)
    {
        var cpf = _cpfProtector.Desproteger(registro.CpfCifrado);

        return Responsavel.Reconstituir(
            id: registro.Id,
            nome: registro.Nome,
            cpf: cpf);
    }

    public Task<bool> ExisteCpfAsync(
    Cpf cpf,
    CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(cpf);

        var hash = _cpfProtector.CalcularHash(cpf);
        return CpfHashExisteAsync(hash, ct);
    }

    private Task<bool> CpfHashExisteAsync(
    byte[] hash,
    CancellationToken ct)
    {
        return _context.Responsaveis
            .AsNoTracking()
            .AnyAsync(x => x.CpfHash == hash, ct);
    }

    private static bool ViolacaoDeUnicidadeCpf(
        DbUpdateException exception,
        string constraintName)
    {
        return exception.InnerException is PostgresException postgresException
            && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
            && postgresException.ConstraintName == constraintName;
    }
}