using AlunoApi.Application.Security;
using AlunoApi.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "A connection string 'DefaultConnection' não foi configurada.");
        }

        var chaveCriptografia = LerChaveBase64(
            configuration,
            "Cpf:EncryptionKey");

        var chaveHmac = LerChaveBase64(
            configuration,
            "Cpf:HmacKey");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddSingleton<ICpfProtector>(
            new AesGcmCpfProtector(chaveCriptografia, chaveHmac));

        return services;
    }

    private static byte[] LerChaveBase64(
        IConfiguration configuration,
        string nome)
    {
        var valor = configuration[nome];

        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new InvalidOperationException(
                $"A configuração obrigatória '{nome}' não foi definida.");
        }

        try
        {
            return Convert.FromBase64String(valor);
        }
        catch (FormatException exception)
        {
            throw new InvalidOperationException(
                $"A configuração '{nome}' deve estar em Base64.",
                exception);
        }
    }
}