using AlunoApi.Infrastructure.Security;

namespace AlunoApi.Tests.Integration.Repositories;

internal static class TestCpfProtectorFactory
{
    public static AesGcmCpfProtector Criar() =>
        new(
            Enumerable.Range(1, 32).Select(x => (byte)x).ToArray(),
            Enumerable.Range(33, 32).Select(x => (byte)x).ToArray());
}