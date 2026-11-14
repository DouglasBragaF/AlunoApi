# AlunoApi: Arquitetura

> **Status:** Etapa 0 em andamento · **Atualizado em:** 2026-11-14
> Este documento descreve a arquitetura atual. Os motivos das escolhas
> estão nos [ADRs](adr/README.md); o que ainda será feito está no [roadmap](roadmap.md).

## Escopo
- Login com JWT
- Cadastro, edição, consulta e inativação de aluno
- Vínculo N:N entre alunos e responsáveis

## Visão geral
Clean Architecture em monólito, com um único Bounded Context
([ADR-0001](adr/0001-clean-architecture-monolito.md)).


```mermaid
flowchart LR SPA[React] -->|JWT| API API --> APP[Application] APP --> DOM[Domain] INF[Infrastructure] --> APP INF --> DB[(PostgreSQL)]

```


| Camada | Responsabilidade |
|---|---|
| Domain | Entidades, Value Objects, regras de negócio, contratos dos repositórios |
| Application | Casos de uso, DTOs, validação (FluentValidation) |
| Infrastructure | EF Core, scripts SQL, modelos de persistência, repositórios, proteção de CPF |
| Api | Controllers, autenticação JWT, OpenAPI |

Estrutura de pastas: [ADR-0002](adr/0002-estrutura-de-pastas.md).

## Domínio
- `Aluno` e `Responsavel` ligados por `AlunoResponsavel`
- `Documento` (Value Object): CPF validado; RG para aluno menor de idade
- Aluno menor de 18 anos exige ao menos 1 responsável
- Matrícula no formato `AAAA` + 6 dígitos, gerada por tabela de contadores ([ADR-0005](adr/0005-matricula-por-contador.md))
- Inativação por `status`, sem exclusão física ([ADR-0006](adr/0006-inativacao-logica.md))

## Persistência
- Banco PostgreSQL criado por scripts SQL versionados, sem migrations ([ADR-0007](adr/0007-schema-por-scripts-sql.md))
- Scripts em `AlunoApi.Infrastructure/Database`
- EF Core apenas mapeia o schema existente, com Fluent API
- Modelos de persistência separados das entidades do Domain ([ADR-0008](adr/0008-modelos-de-persistencia-separados.md))

| Tabela | Descrição |
|---|---|
| `alunos` | Dados do aluno, CPF protegido e matrícula |
| `responsaveis` | Dados do responsável e CPF protegido |
| `alunos_responsaveis` | Vínculo N:N, com chave composta |
| `matricula_contadores` | Última sequência de matrícula por ano |
| `usuarios` | Usuários da autenticação |

## Segurança
- Autenticação JWT (Bearer), senhas com BCrypt ([ADR-0003](adr/0003-autenticacao-jwt.md))
- CPF protegido em duas colunas: AES-GCM (exibição mascarada) e HMAC-SHA256
  (busca e unicidade) ([ADR-0004](adr/0004-protecao-de-cpf.md))
- Segredos fora do repositório: variáveis de ambiente, User Secrets e `.env` ignorado
- Sem PII em logs
- Dados 100% fictícios (LGPD)

## Stack
- **Back:** .NET 10, ASP.NET Core, EF Core, Npgsql, PostgreSQL
- **Front:** React, TypeScript, Vite, TanStack Query
- **Infra:** Docker, Docker Compose
- **Erros:** `ProblemDetails` (RFC 7807)

## Estratégia de testes
- Unitários no domínio (xUnit), com TDD
- Integração com Testcontainers e PostgreSQL, com um container compartilhado entre os testes
- Unitários da Application com Moq (planejado)
- Arquitetura com NetArchTest (planejado)