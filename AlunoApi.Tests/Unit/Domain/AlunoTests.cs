namespace AlunoApi.Tests.Unit.Domain;

public sealed class AlunoTests
{
    [Fact]
    public void Deve_criar_aluno_com_dados_validos()
    {
        var responsavelId = Guid.NewGuid();
        var documento = Documento.Criar("52998224725", "123456");
        var matricula = Matricula.Criar(2026, 1);

        var aluno = Aluno.Criar(
            nome: "Aluno Teste",
            dataNascimento: new DateOnly(2012, 5, 10),
            documento: documento,
            matricula: matricula,
            responsaveisIds: [responsavelId],
            hoje: new DateOnly(2026, 1, 1));

        Assert.NotEqual(Guid.Empty, aluno.Id);
        Assert.Equal("Aluno Teste", aluno.Nome);
        Assert.Equal(new DateOnly(2012, 5, 10), aluno.DataNascimento);
        Assert.Equal("2026000001", aluno.Matricula.Valor);
        Assert.Equal(StatusAluno.Ativo, aluno.Status);
        Assert.Single(aluno.Responsaveis);
        Assert.True(aluno.PossuiResponsavel(responsavelId));
        Assert.True(aluno.EstaAtivo);
    }

    [Fact]
    public void Deve_reconstituir_aluno_preservando_id_status_e_vinculos()
    {
        var alunoId = Guid.NewGuid();
        var responsavelId1 = Guid.NewGuid();
        var responsavelId2 = Guid.NewGuid();
        var documento = Documento.Criar("52998224725", "123456");
        var matricula = Matricula.Criar(2026, 123);

        var aluno = Aluno.Reconstituir(
            id: alunoId,
            nome: "Aluno Teste",
            dataNascimento: new DateOnly(2012, 5, 10),
            documento: documento,
            matricula: matricula,
            status: StatusAluno.Inativo,
            responsaveisIds: [responsavelId1, responsavelId2]);

        Assert.Equal(alunoId, aluno.Id);
        Assert.Equal("Aluno Teste", aluno.Nome);
        Assert.Equal(new DateOnly(2012, 5, 10), aluno.DataNascimento);
        Assert.Equal(documento, aluno.Documento);
        Assert.Equal(matricula, aluno.Matricula);
        Assert.Equal(StatusAluno.Inativo, aluno.Status);
        Assert.False(aluno.EstaAtivo);

        Assert.Equal(2, aluno.Responsaveis.Count);
        Assert.True(aluno.PossuiResponsavel(responsavelId1));
        Assert.True(aluno.PossuiResponsavel(responsavelId2));
        Assert.All(aluno.Responsaveis, vinculo =>
            Assert.Equal(alunoId, vinculo.AlunoId));
    }

    [Fact]
    public void Deve_rejeitar_id_vazio_ao_reconstituir_aluno()
    {
        Assert.Throws<DomainException>(() =>
            Aluno.Reconstituir(
                id: Guid.Empty,
                nome: "Aluno Teste",
                dataNascimento: new DateOnly(2012, 5, 10),
                documento: Documento.Criar("52998224725", "123456"),
                matricula: Matricula.Criar(2026, 1),
                status: StatusAluno.Ativo,
                responsaveisIds: [Guid.NewGuid()]));
    }

    [Fact]
    public void Deve_rejeitar_status_invalido_ao_reconstituir_aluno()
    {
        Assert.Throws<DomainException>(() =>
            Aluno.Reconstituir(
                id: Guid.NewGuid(),
                nome: "Aluno Teste",
                dataNascimento: new DateOnly(2012, 5, 10),
                documento: Documento.Criar("52998224725", "123456"),
                matricula: Matricula.Criar(2026, 1),
                status: (StatusAluno)99,
                responsaveisIds: [Guid.NewGuid()]));
    }

    [Fact]
    public void Deve_rejeitar_id_de_responsavel_vazio_ao_reconstituir_aluno()
    {
        Assert.Throws<DomainException>(() =>
            Aluno.Reconstituir(
                id: Guid.NewGuid(),
                nome: "Aluno Teste",
                dataNascimento: new DateOnly(2012, 5, 10),
                documento: Documento.Criar("52998224725", "123456"),
                matricula: Matricula.Criar(2026, 1),
                status: StatusAluno.Ativo,
                responsaveisIds: [Guid.Empty]));
    }
}