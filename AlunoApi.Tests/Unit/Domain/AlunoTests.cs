namespace AlunoApi.Tests.Unit.Domain;

public sealed class AlunoTests
{
    [Fact]
    public void Deve_criar_aluno_com_dados_validos()
    {
        var responsavelId = Guid.NewGuid();
        var documento = Documento.Criar(
            "52998224725",
            "123456");

        var matricula = Matricula.Criar(
            2026,
            1);

        var aluno = Aluno.Criar(
            nome: "Aluno Teste",
            dataNascimento: new DateOnly(2012, 5, 10),
            documento: documento,
            matricula: matricula,
            responsaveisIds: [responsavelId],
            hoje: new DateOnly(2026, 1, 1));

        Assert.NotEqual(Guid.Empty, aluno.Id);
        Assert.Equal("Aluno Teste", aluno.Nome);
        Assert.Equal(
            new DateOnly(2012, 5, 10),
            aluno.DataNascimento);

        Assert.Equal("2026000001", aluno.Matricula.Valor);
        Assert.Equal(StatusAluno.Ativo, aluno.Status);

        Assert.Single(aluno.Responsaveis);
        Assert.True(aluno.PossuiResponsavel(responsavelId));
        Assert.True(aluno.EstaAtivo);
    }
}