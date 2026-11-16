using AlunoApi.Infrastructure.Repositories;
using AlunoApi.Tests.Integration.Database;
using Microsoft.EntityFrameworkCore;

namespace AlunoApi.Tests.Integration.Repositories;

[Collection("PostgresDatabase")]
public sealed class ResponsavelRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;

    public ResponsavelRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Deve_persistir_e_reconstituir_responsavel()
    {
        await _fixture.LimparBancoAsync();

        var protector = TestCpfProtectorFactory.Criar();

        await using var context = _fixture.CreateContext();
        var repository = new ResponsavelRepository(context, protector);

        var responsavel = Responsavel.Criar(
            "Responsável Teste",
            Cpf.Criar("52998224725"));

        await repository.AdicionarAsync(responsavel);

        var obtido = await repository.ObterPorIdAsync(responsavel.Id);

        Assert.NotNull(obtido);
        Assert.Equal(responsavel.Id, obtido.Id);
        Assert.Equal("Responsável Teste", obtido.Nome);
        Assert.Equal("52998224725", obtido.Cpf.Valor);

        var registro = await context.Responsaveis
            .AsNoTracking()
            .SingleAsync(x => x.Id == responsavel.Id);

        Assert.Equal(39, registro.CpfCifrado.Length);
        Assert.Equal(32, registro.CpfHash.Length);
    }

    [Fact]
    public async Task Deve_retornar_nulo_quando_responsavel_nao_existir()
    {
        await _fixture.LimparBancoAsync();

        await using var context = _fixture.CreateContext();
        var repository = new ResponsavelRepository(
            context,
            TestCpfProtectorFactory.Criar());

        var resultado = await repository.ObterPorIdAsync(Guid.NewGuid());

        Assert.Null(resultado);
    }
}