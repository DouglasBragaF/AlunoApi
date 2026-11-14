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
}