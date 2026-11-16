using Microsoft.EntityFrameworkCore;

namespace AlunoApi.Tests.Integration.Database;

[Collection("PostgresDatabase")]
public sealed class DatabaseConnectionTests
{
    private readonly PostgresDatabaseFixture _fixture;

    public DatabaseConnectionTests(
        PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Deve_conectar_ao_banco_de_dados()
    {
        await using var context = _fixture.CreateContext();

        var conectado = await context.Database.CanConnectAsync();

        Assert.True(conectado);
    }

    [Fact]
    public async Task Deve_acessar_as_tabelas_principais()
    {
        await using var context = _fixture.CreateContext();
        await _fixture.LimparBancoAsync();

        Assert.False(await context.Alunos.AnyAsync());
        Assert.False(await context.Responsaveis.AnyAsync());
        Assert.False(await context.AlunosResponsaveis.AnyAsync());
    }
}