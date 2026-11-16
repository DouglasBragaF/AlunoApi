namespace AlunoApi.Application.Security;

public interface ICpfProtector
{
    ProtectedCpf Proteger(global::Cpf cpf);

    global::Cpf Desproteger(byte[] cpfCifrado);
}