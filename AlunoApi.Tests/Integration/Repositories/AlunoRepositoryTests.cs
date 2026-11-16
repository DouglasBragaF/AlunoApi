using AlunoApi.Infrastructure.Repositories;
using AlunoApi.Tests.Integration.Database;
using Microsoft.EntityFrameworkCore;

namespace AlunoApi.Tests.Integration.Repositories;

[Collection("PostgresDatabase")]
public sealed class AlunoRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;

    public AlunoRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Deve_persistir_e_reconstituir_aluno_com_responsaveis()
    {
        await _fixture.LimparBancoAsync();

        var protector = TestCpfProtectorFactory.Criar();

        await using var context = _fixture.CreateContext();

        var responsavelRepository =
            new ResponsavelRepository(context, protector);

        var responsavel = Responsavel.Criar(
            "Responsável Teste",
            Cpf.Criar("52998224725"));

        await responsavelRepository.AdicionarAsync(responsavel);

        var repository = new AlunoRepository(context, protector);
        var responsavelId = responsavel.Id;
        var dataNascimento = new DateOnly(2012, 5, 10);
        var documento = Documento.Criar("11144477735", "123456");
        var matricula = Matricula.Criar(2026, 1);

        var aluno = Aluno.Criar(
            nome: "Aluno Teste",
            dataNascimento: dataNascimento,
            documento: documento,
            matricula: matricula,
            responsaveisIds: [responsavelId],
            hoje: new DateOnly(2026, 1, 1));

        aluno.Inativar();

        await repository.AdicionarAsync(aluno);

        var obtido = await repository.ObterPorIdAsync(aluno.Id);

        Assert.NotNull(obtido);
        Assert.Equal(aluno.Id, obtido.Id);
        Assert.Equal("Aluno Teste", obtido.Nome);
        Assert.Equal(dataNascimento, obtido.DataNascimento);
        Assert.Equal("11144477735", obtido.Documento.Cpf.Valor);
        Assert.Equal("123456", obtido.Documento.Rg!.Valor);
        Assert.Equal("2026000001", obtido.Matricula.Valor);
        Assert.Equal(StatusAluno.Inativo, obtido.Status);
        Assert.True(obtido.PossuiResponsavel(responsavelId));

        var registro = await context.Alunos
            .AsNoTracking()
            .SingleAsync(x => x.Id == aluno.Id);

        Assert.Equal(39, registro.CpfCifrado.Length);
        Assert.Equal(32, registro.CpfHash.Length);
    }

    [Fact]
    public async Task Deve_retornar_nulo_quando_aluno_nao_existir()
    {
        await _fixture.LimparBancoAsync();

        await using var context = _fixture.CreateContext();
        var repository = new AlunoRepository(
            context,
            TestCpfProtectorFactory.Criar());

        var resultado = await repository.ObterPorIdAsync(Guid.NewGuid());

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Deve_reservar_sequencias_unicas_em_chamadas_concorrentes()
    {
        await _fixture.LimparBancoAsync();

        const int ano = 2098;
        const int quantidade = 10;
        var protector = TestCpfProtectorFactory.Criar();

        var tarefas = Enumerable.Range(0, quantidade).Select(async _ =>
        {
            await using var context = _fixture.CreateContext();
            var repository = new AlunoRepository(context, protector);

            return await repository.ObterProximaSequenciaMatriculaAsync(ano);
        });

        var sequencias = await Task.WhenAll(tarefas);

        Assert.Equal(
            Enumerable.Range(1, quantidade),
            sequencias.OrderBy(x => x));
    }

    [Fact]
    public async Task Deve_reiniciar_sequencia_para_outro_ano()
    {
        await _fixture.LimparBancoAsync();

        await using var context = _fixture.CreateContext();
        var repository = new AlunoRepository(
            context,
            TestCpfProtectorFactory.Criar());

        Assert.Equal(
            1,
            await repository.ObterProximaSequenciaMatriculaAsync(2097));

        Assert.Equal(
            1,
            await repository.ObterProximaSequenciaMatriculaAsync(2098));
    }

    [Fact]
    public async Task Deve_rejeitar_sequencia_esgotada()
    {
        await _fixture.LimparBancoAsync();

        const int ano = 2099;

        await using var context = _fixture.CreateContext();

        await context.Database.ExecuteSqlInterpolatedAsync(
            $"INSERT INTO public.matricula_contadores (ano, ultima_sequencia) VALUES ({ano}, 999999)");

        var repository = new AlunoRepository(
            context,
            TestCpfProtectorFactory.Criar());

        await Assert.ThrowsAsync<DomainException>(
            () => repository.ObterProximaSequenciaMatriculaAsync(ano));
    }
}