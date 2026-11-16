namespace AlunoApi.Tests.Unit.Domain;

public sealed class DocumentoTests
{
    [Fact]
    public void Deve_criar_documento_com_cpf_e_rg_validos()
    {
        var documento = Documento.Criar("529.982.247-25", "12.345.678-9");

        Assert.Equal("52998224725", documento.Cpf.Valor);
        Assert.True(documento.PossuiRg);
        Assert.Equal("123456789", documento.Rg!.Valor);
    }

    [Fact]
    public void Deve_criar_documento_sem_rg()
    {
        var documento = Documento.Criar("52998224725");

        Assert.Equal("52998224725", documento.Cpf.Valor);
        Assert.False(documento.PossuiRg);
        Assert.Null(documento.Rg);
    }

    [Fact]
    public void Deve_rejeitar_cpf_invalido()
    {
        Assert.Throws<DomainException>(
            () => Documento.Criar("11111111111"));
    }

    [Fact]
    public void Deve_rejeitar_rg_curto()
    {
        Assert.Throws<DomainException>(
            () => Documento.Criar("52998224725", "1234"));
    }
}