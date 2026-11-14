# AlunoApi

API REST em .NET para cadastro e consulta de alunos e seus responsáveis, com front-end em React.

## Objetivo

Demonstrar, em um domínio pequeno, como tratar dados pessoais sensíveis (CPF e RG) com Clean Architecture, DDD e TDD: regras de negócio isoladas no domínio, proteção de dados na infraestrutura e decisões registradas em ADRs.

> **Dados 100% fictícios.** O projeto não deve receber informações reais (LGPD).

## Regras de negócio

- Aluno e responsável se relacionam em N:N.
- Aluno menor de 18 anos exige ao menos um responsável.
- CPF é validado; o RG é exigido para aluno menor de idade.
- A matrícula segue o formato `AAAA` + sequência, gerada por sequence no banco.
- Aluno não é excluído: é inativado por `status`.

## Segurança

- Autenticação JWT (Bearer), com senhas protegidas por BCrypt.
- O CPF é armazenado em duas colunas: uma criptografada com AES-GCM (exibida mascarada) e outra com HMAC-SHA256 (busca e unicidade, sem expor o valor).
- Nenhum dado pessoal é registrado em logs.
- Erros seguem o padrão `ProblemDetails` (RFC 7807).

## Arquitetura

Monólito em Clean Architecture, com quatro camadas: Domain, Application, Infrastructure e Api. As dependências apontam para o domínio.

```
AlunoApi/
├── src/
│   ├── AlunoApi.Domain/
│   ├── AlunoApi.Application/
│   ├── AlunoApi.Infrastructure/
│   ├── AlunoApi.Api/
│   └── AlunoApi.Front/
├── tests/
│   ├── AlunoApi.UnitTests/
│   ├── AlunoApi.IntegrationTests/
│   └── AlunoApi.ArchitectureTests/
└── docs/
    ├── architecture.md
    ├── roadmap.md
    └── adr/
```

Stack: .NET 10, ASP.NET Core, EF Core, PostgreSQL, React, TypeScript, Vite e TanStack Query.

## Como executar

Pré-requisitos: .NET SDK 10, Node.js LTS e Docker.

```bash
git clone https://github.com/seu-usuario/AlunoApi.git
cd AlunoApi

docker compose up -d db
cp .env.example .env

dotnet run --project src/AlunoApi.Api

# em outro terminal
cd src/AlunoApi.Front
npm install
npm run dev
```

O Swagger fica em `/swagger`.

## Configuração

Use variáveis de ambiente, *user secrets* ou `.env` (que não é versionado). O `.env.example` lista as chaves.

| Variável | Uso |
|---|---|
| `ConnectionStrings__DefaultConnection` | Conexão com o PostgreSQL |
| `Jwt__Key`, `Jwt__Issuer`, `Jwt__Audience` | Geração e validação do token |
| `Cpf__EncryptionKey` | Chave AES-GCM do CPF |
| `Cpf__HmacKey` | Chave HMAC-SHA256 do CPF |

## Testes

```bash
dotnet test
```

- **Unitários:** domínio, com TDD (xUnit, Moq, FluentAssertions).
- **Integração:** Testcontainers (exige Docker em execução).
- **Arquitetura:** NetArchTest, para garantir que as camadas respeitem as dependências.

## Documentação

- [Arquitetura](docs/architecture.md)
- [Decisões de arquitetura (ADRs)](docs/adr/README.md)
- [Roadmap](docs/roadmap.md): CI com GitHub Actions, Docker Compose completo e deploy no Azure estão planejados e descritos aqui.