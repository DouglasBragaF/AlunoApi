namespace AlunoApi.Application.Security;

public sealed record ProtectedCpf(byte[] Cifrado, byte[] Hash);