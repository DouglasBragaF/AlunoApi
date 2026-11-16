using System;

public sealed class Aluno : AggregateRoot
{
    public const int Maioridade = 18;

    private readonly List<AlunoResponsavel> _responsaveis = [];

    public string Nome { get; private set; } = default!;
    public DateOnly DataNascimento { get; private set; }
    public Documento Documento { get; private set; } = default!;
    public Matricula Matricula { get; private set; } = default!;
    public StatusAluno Status { get; private set; }
    public IReadOnlyCollection<AlunoResponsavel> Responsaveis => _responsaveis.AsReadOnly();

    public bool EstaAtivo => Status == StatusAluno.Ativo;

    private Aluno() { } // EF Core

    // ---------- Criação ----------
    public static Aluno Criar(
        string nome, DateOnly dataNascimento, Documento documento,
        Matricula matricula, IEnumerable<Guid> responsaveisIds, DateOnly hoje)
    {
        ArgumentNullException.ThrowIfNull(documento);
        ArgumentNullException.ThrowIfNull(matricula);

        var nomeValido = Guard.AgainstNullOrWhiteSpace(nome, "Nome");
        var ids = responsaveisIds.Distinct().ToList();
        ValidarRegras(dataNascimento, documento, ids.Count, hoje);

        var aluno = new Aluno
        {
            Nome = nomeValido,
            DataNascimento = dataNascimento,
            Documento = documento,
            Matricula = matricula,
            Status = StatusAluno.Ativo
        };
        ids.ForEach(id => aluno._responsaveis.Add(new AlunoResponsavel(aluno.Id, id)));
        return aluno;
    }

    // ---------- Consultas ----------
    public int CalcularIdade(DateOnly hoje) => DataNascimento.CalcularIdade(hoje);
    public bool EhMenor(DateOnly hoje) => CalcularIdade(hoje) < Maioridade;
    public bool PossuiResponsavel(Guid responsavelId) =>
        _responsaveis.Any(r => r.ResponsavelId == responsavelId);

    // ---------- Comandos ----------
    public void AtualizarDados(string nome, DateOnly dataNascimento, Documento documento, DateOnly hoje)
    {
        ArgumentNullException.ThrowIfNull(documento);
        GarantirAtivo();

        var nomeValido = Guard.AgainstNullOrWhiteSpace(nome, "Nome");
        ValidarRegras(dataNascimento, documento, _responsaveis.Count, hoje);

        Nome = nomeValido;
        DataNascimento = dataNascimento;
        Documento = documento;
    }

    public void VincularResponsavel(Guid responsavelId)
    {
        GarantirAtivo();
        Guard.Against(responsavelId == Guid.Empty, "Responsável inválido.");

        if (PossuiResponsavel(responsavelId)) return; // idempotente
        _responsaveis.Add(new AlunoResponsavel(Id, responsavelId));
    }

    public void DesvincularResponsavel(Guid responsavelId, DateOnly hoje)
    {
        GarantirAtivo();

        var vinculo = _responsaveis.Find(r => r.ResponsavelId == responsavelId)
            ?? throw new DomainException("Responsável não vinculado ao aluno.");

        ValidarRegras(DataNascimento, Documento, _responsaveis.Count - 1, hoje);
        _responsaveis.Remove(vinculo);
    }

    public void Inativar()
    {
        Guard.Against(!EstaAtivo, "Aluno já está inativo.");
        Status = StatusAluno.Inativo;
    }

    // ---------- Regras (única fonte de verdade) ----------
    private void GarantirAtivo() =>
        Guard.Against(!EstaAtivo, "Aluno inativo não pode ser alterado.");

    private static void ValidarRegras(
        DateOnly dataNascimento, Documento documento, int totalResponsaveis, DateOnly hoje)
    {
        Guard.Against(dataNascimento > hoje, "Data de nascimento não pode ser futura.");

        if (dataNascimento.CalcularIdade(hoje) >= Maioridade) return;

        Guard.Against(!documento.PossuiRg, "RG é obrigatório para aluno menor de idade.");
        Guard.Against(totalResponsaveis == 0, "Aluno menor de idade exige ao menos um responsável.");
    }

    public static Aluno Reconstituir(
    Guid id,
    string nome,
    DateOnly dataNascimento,
    Documento documento,
    Matricula matricula,
    StatusAluno status,
    IEnumerable<Guid> responsaveisIds)
    {
        Guard.Against(id == Guid.Empty, "Id do aluno inválido.");
        ArgumentNullException.ThrowIfNull(documento);
        ArgumentNullException.ThrowIfNull(matricula);
        ArgumentNullException.ThrowIfNull(responsaveisIds);
        Guard.Against(
            !Enum.IsDefined(typeof(StatusAluno), status),
            "Status do aluno inválido.");

        var ids = responsaveisIds.Distinct().ToArray();
        Guard.Against(
            ids.Any(responsavelId => responsavelId == Guid.Empty),
            "Id de responsável inválido.");

        var aluno = new Aluno
        {
            Id = id,
            Nome = Guard.AgainstNullOrWhiteSpace(nome, "Nome"),
            DataNascimento = dataNascimento,
            Documento = documento,
            Matricula = matricula,
            Status = status
        };

        foreach (var responsavelId in ids)
            aluno._responsaveis.Add(new AlunoResponsavel(id, responsavelId));

        return aluno;
    }
}