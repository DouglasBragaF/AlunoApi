using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;

namespace AlunoApi.Tests.Integration.Database;
using AlunoApi.Infrastructure.Repositories;

public sealed class PostgresDatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container =
        new PostgreSqlBuilder("postgres:17")
            .WithDatabase("alunoapi_test")
            .WithUsername("alunoapi_test")
            .WithPassword("alunoapi_test_password")
            .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        await using var connection =
            new NpgsqlConnection(ConnectionString);

        await connection.OpenAsync();

        var scriptPath = Path.Combine(
            AppContext.BaseDirectory,
            "Database",
            "001_initial_schema.sql");

        var script = await File.ReadAllTextAsync(scriptPath);

        await using var command = new NpgsqlCommand(script, connection);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new AppDbContext(options);
    }

    public async Task LimparBancoAsync()
    {
        await using var context = CreateContext();

        await context.Database.ExecuteSqlRawAsync("""
        TRUNCATE TABLE
            public.alunos_responsaveis,
            public.alunos,
            public.responsaveis,
            public.usuarios,
            public.matricula_contadores
        RESTART IDENTITY CASCADE;
        """);
    }
}