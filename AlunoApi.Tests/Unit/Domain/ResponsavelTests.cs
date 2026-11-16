namespace AlunoApi.Tests.Unit.Domain;

public sealed class ResponsavelTests
{
    [Fact]
    public void Deve_criar_responsavel_com_dados_validos()
    {
        var cpf = Cpf.Criar("52998224725");

        var responsavel = Responsavel.Criar(
            nome: "Responsável Teste",
            cpf: cpf);

        Assert.NotEqual(Guid.Empty, responsavel.Id);
        Assert.Equal("Responsável Teste", responsavel.Nome);
        Assert.Equal(cpf, responsavel.Cpf);
    }

    [Fact]
    public void Deve_reconstituir_responsavel_preservando_id_e_dados()
    {
        var id = Guid.NewGuid();
        var cpf = Cpf.Criar("52998224725");

        var responsavel = Responsavel.Reconstituir(
            id,
            "Responsável Teste",
            cpf);

        Assert.Equal(id, responsavel.Id);
        Assert.Equal("Responsável Teste", responsavel.Nome);
        Assert.Equal(cpf, responsavel.Cpf);
    }

    [Fact]
    public void Deve_rejeitar_id_vazio_ao_reconstituir_responsavel()
    {
        var cpf = Cpf.Criar("52998224725");

        Assert.Throws<DomainException>(() =>
            Responsavel.Reconstituir(
                Guid.Empty,
                "Responsável Teste",
                cpf));
    }

    [Fact]
    public void Deve_rejeitar_nome_vazio_ao_reconstituir_responsavel()
    {
        var cpf = Cpf.Criar("52998224725");

        Assert.Throws<DomainException>(() =>
            Responsavel.Reconstituir(
                Guid.NewGuid(),
                " ",
                cpf));
    }
}