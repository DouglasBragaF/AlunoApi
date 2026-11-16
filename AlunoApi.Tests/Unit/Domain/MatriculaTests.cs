namespace AlunoApi.Tests.Unit.Domain;

public sealed class MatriculaTests
{
    [Fact]
    public void Deve_criar_matricula_com_formato_ano_e_seis_digitos()
    {
        var matricula = Matricula.Criar(2026, 1);

        Assert.Equal(2026, matricula.Ano);
        Assert.Equal(1, matricula.Sequencia);
        Assert.Equal("2026000001", matricula.Valor);
        Assert.Equal(matricula.Valor, matricula.ToString());
    }

    [Fact]
    public void Deve_reconstruir_matricula_a_partir_do_valor()
    {
        var matricula = Matricula.Parse("2026000123");

        Assert.Equal(2026, matricula.Ano);
        Assert.Equal(123, matricula.Sequencia);
        Assert.Equal("2026000123", matricula.Valor);
    }

    [Fact]
    public void Deve_aceitar_sequencia_maxima_de_seis_digitos()
    {
        var matricula = Matricula.Criar(2026, 999_999);

        Assert.Equal("2026999999", matricula.Valor);
    }

    [Fact]
    public void Deve_rejeitar_sequencia_maior_que_seis_digitos()
    {
        Assert.Throws<DomainException>(
            () => Matricula.Criar(2026, 1_000_000));
    }

    [Theory]
    [InlineData(1999)]
    [InlineData(3000)]
    public void Deve_rejeitar_ano_invalido(int ano)
    {
        Assert.Throws<DomainException>(
            () => Matricula.Criar(ano, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Deve_rejeitar_sequencia_nao_positiva(int sequencia)
    {
        Assert.Throws<DomainException>(
            () => Matricula.Criar(2026, sequencia));
    }

    [Theory]
    [InlineData("202600123")]
    [InlineData("20260A0123")]
    public void Deve_rejeitar_texto_de_matricula_invalido(string valor)
    {
        Assert.Throws<DomainException>(
            () => Matricula.Parse(valor));
    }
}