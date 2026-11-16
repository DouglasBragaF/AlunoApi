using AlunoApi.Application.Security;
using Microsoft.EntityFrameworkCore;

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

        var registro = new ResponsavelPersistence
        {
            Id = responsavel.Id,
            Nome = responsavel.Nome,
            CpfCifrado = cpfProtegido.Cifrado,
            CpfHash = cpfProtegido.Hash
        };

        _context.Responsaveis.Add(registro);
        await _context.SaveChangesAsync(ct);
    }

    private Responsavel ParaDominio(ResponsavelPersistence registro)
    {
        var cpf = _cpfProtector.Desproteger(registro.CpfCifrado);

        return Responsavel.Reconstituir(
            id: registro.Id,
            nome: registro.Nome,
            cpf: cpf);
    }
}