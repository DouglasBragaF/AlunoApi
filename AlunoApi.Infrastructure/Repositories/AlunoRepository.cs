using System.Data;
using AlunoApi.Application.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

namespace AlunoApi.Infrastructure.Repositories;

public sealed class AlunoRepository : IAlunoRepository
{
    private readonly AppDbContext _context;
    private readonly ICpfProtector _cpfProtector;

    public AlunoRepository(
        AppDbContext context,
        ICpfProtector cpfProtector)
    {
        _context = context;
        _cpfProtector = cpfProtector;
    }

    public async Task<Aluno?> ObterPorIdAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var registro = await _context.Alunos
            .AsNoTracking()
            .Include(x => x.Responsaveis)
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        return registro is null ? null : ParaDominio(registro);
    }

    public async Task<int> ObterProximaSequenciaMatriculaAsync(
        int ano,
        CancellationToken ct = default)
    {
        if (ano is < 2000 or > 2999)
            throw new DomainException("Ano da matrícula inválido.");

        var connection = _context.Database.GetDbConnection();
        var fecharConexao = connection.State != ConnectionState.Open;

        if (fecharConexao)
            await _context.Database.OpenConnectionAsync(ct);

        try
        {
            await using var command = connection.CreateCommand();

            command.CommandText = """
                INSERT INTO public.matricula_contadores AS contador
                    (ano, ultima_sequencia)
                VALUES
                    (@ano, 1)
                ON CONFLICT (ano) DO UPDATE
                SET ultima_sequencia = contador.ultima_sequencia + 1
                WHERE contador.ultima_sequencia < 999999
                RETURNING ultima_sequencia;
                """;

            command.Transaction = _context.Database
                .CurrentTransaction?
                .GetDbTransaction();

            var parametroAno = command.CreateParameter();
            parametroAno.ParameterName = "ano";
            parametroAno.DbType = DbType.Int16;
            parametroAno.Value = checked((short)ano);
            command.Parameters.Add(parametroAno);

            var resultado = await command.ExecuteScalarAsync(ct);

            if (resultado is null || resultado == DBNull.Value)
                throw new DomainException(
                    "A sequência de matrículas deste ano foi esgotada.");

            return Convert.ToInt32(resultado);
        }
        finally
        {
            if (fecharConexao)
                await _context.Database.CloseConnectionAsync();
        }
    }

    public async Task AdicionarAsync(
    Aluno aluno,
    CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(aluno);

        var cpfProtegido = _cpfProtector.Proteger(aluno.Documento.Cpf);

        if (await CpfHashExisteAsync(cpfProtegido.Hash, ct))
            throw new DomainException(
                "Já existe um aluno cadastrado com este CPF.");

        var registro = new AlunoPersistence
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            DataNascimento = aluno.DataNascimento,
            CpfCifrado = cpfProtegido.Cifrado,
            CpfHash = cpfProtegido.Hash,
            Rg = aluno.Documento.Rg?.Valor,
            MatriculaAno = checked((short)aluno.Matricula.Ano),
            MatriculaSequencia = aluno.Matricula.Sequencia,
            Matricula = aluno.Matricula.Valor,
            Status = (short)aluno.Status,
            Responsaveis = aluno.Responsaveis
                .Select(vinculo => new AlunoResponsavelPersistence
                {
                    AlunoId = aluno.Id,
                    ResponsavelId = vinculo.ResponsavelId
                })
                .ToList()
        };

        _context.Alunos.Add(registro);

        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (
            ViolacaoDeUnicidadeCpf(ex, "uq_alunos_cpf_hash"))
        {
            throw new DomainException(
                "Já existe um aluno cadastrado com este CPF.");
        }
    }

    private Task<bool> CpfHashExisteAsync(
        byte[] hash,
        CancellationToken ct)
    {
        return _context.Alunos
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

    private Aluno ParaDominio(AlunoPersistence registro)
    {
        var cpf = _cpfProtector.Desproteger(registro.CpfCifrado);
        var rg = registro.Rg is null ? null : Rg.Criar(registro.Rg);
        var documento = Documento.Criar(cpf, rg);
        var matricula = Matricula.Criar(
            registro.MatriculaAno,
            registro.MatriculaSequencia);

        if (matricula.Valor != registro.Matricula)
            throw new InvalidOperationException(
                $"A matrícula persistida do aluno {registro.Id} é inconsistente.");

        return Aluno.Reconstituir(
            id: registro.Id,
            nome: registro.Nome,
            dataNascimento: registro.DataNascimento,
            documento: documento,
            matricula: matricula,
            status: (StatusAluno)registro.Status,
            responsaveisIds: registro.Responsaveis
                .Select(vinculo => vinculo.ResponsavelId));
    }

    public Task<bool> ExisteCpfAsync(
    Cpf cpf,
    CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(cpf);

        var hash = _cpfProtector.CalcularHash(cpf);
        return CpfHashExisteAsync(hash, ct);
    }
}