using System.Security.Cryptography;
using System.Text;
using AlunoApi.Application.Security;

namespace AlunoApi.Infrastructure.Security;

public sealed class AesGcmCpfProtector : ICpfProtector
{
    private const int TamanhoNonce = 12;
    private const int TamanhoTag = 16;
    private const int TamanhoCpf = 11;

    private readonly byte[] _chaveCriptografia;
    private readonly byte[] _chaveHmac;

    public AesGcmCpfProtector(byte[] chaveCriptografia, byte[] chaveHmac)
    {
        ArgumentNullException.ThrowIfNull(chaveCriptografia);
        ArgumentNullException.ThrowIfNull(chaveHmac);

        if (chaveCriptografia.Length != 32)
            throw new ArgumentException("A chave AES deve ter 32 bytes.", nameof(chaveCriptografia));

        if (chaveHmac.Length < 32)
            throw new ArgumentException("A chave HMAC deve ter pelo menos 32 bytes.", nameof(chaveHmac));

        _chaveCriptografia = chaveCriptografia.ToArray();
        _chaveHmac = chaveHmac.ToArray();
    }

    public ProtectedCpf Proteger(global::Cpf cpf)
    {
        ArgumentNullException.ThrowIfNull(cpf);

        var texto = Encoding.UTF8.GetBytes(cpf.Valor);

        try
        {
            var nonce = RandomNumberGenerator.GetBytes(TamanhoNonce);
            var tag = new byte[TamanhoTag];
            var cifrado = new byte[texto.Length];

            using var aes = new AesGcm(_chaveCriptografia, TamanhoTag);
            aes.Encrypt(nonce, texto, cifrado, tag);

            // Formato persistido em cpf_cifrado: nonce + tag + conteúdo cifrado.
            var pacote = new byte[TamanhoNonce + TamanhoTag + cifrado.Length];
            nonce.CopyTo(pacote, 0);
            tag.CopyTo(pacote, TamanhoNonce);
            cifrado.CopyTo(pacote, TamanhoNonce + TamanhoTag);

            var hash = HMACSHA256.HashData(_chaveHmac, texto);

            return new ProtectedCpf(pacote, hash);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(texto);
        }
    }

    public global::Cpf Desproteger(byte[] cpfCifrado)
    {
        ArgumentNullException.ThrowIfNull(cpfCifrado);

        var tamanhoEsperado = TamanhoNonce + TamanhoTag + TamanhoCpf;
        if (cpfCifrado.Length != tamanhoEsperado)
            throw new CryptographicException("O conteúdo cifrado do CPF tem formato inválido.");

        var nonce = cpfCifrado.AsSpan(0, TamanhoNonce);
        var tag = cpfCifrado.AsSpan(TamanhoNonce, TamanhoTag);
        var conteudo = cpfCifrado.AsSpan(TamanhoNonce + TamanhoTag);
        var texto = new byte[TamanhoCpf];

        try
        {
            using var aes = new AesGcm(_chaveCriptografia, TamanhoTag);
            aes.Decrypt(nonce, conteudo, tag, texto);

            return global::Cpf.Criar(Encoding.UTF8.GetString(texto));
        }
        finally
        {
            CryptographicOperations.ZeroMemory(texto);
        }
    }

    public byte[] CalcularHash(global::Cpf cpf)
    {
        ArgumentNullException.ThrowIfNull(cpf);

        var texto = Encoding.UTF8.GetBytes(cpf.Valor);

        try
        {
            return HMACSHA256.HashData(_chaveHmac, texto);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(texto);
        }
    }
}