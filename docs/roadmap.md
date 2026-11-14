# AlunoApi: Roadmap

> Este documento lista o que será feito em cada etapa.
> A arquitetura atual está em [architecture.md](architecture.md) e os motivos das decisões estão nos [ADRs](adr/README.md).

## Legenda

- ✅ Concluído
- 🚧 Em andamento
- ⏳ Pendente

## Etapa 0: Fundação 🚧

**Objetivo:** estrutura base validada, do domínio ao banco.

- ✅ Solução com Domain, Application, Infrastructure, Api e Tests
- ✅ Entidades e Value Objects do domínio
- ✅ Script SQL inicial (`001_initial_schema.sql`)
- ✅ PostgreSQL local via Docker Compose
- ✅ `AppDbContext` e mapeamento Fluent API
- ✅ Teste de integração com Testcontainers
- 🚧 Testes unitários do Domain
- ⏳ Modelo e mapeamento de `usuarios` e `matricula_contadores`

**Critério de conclusão:** `dotnet build` e `dotnet test` passam e o banco é criado só pelo script SQL.

## Etapa 1: Persistência ⏳

**Objetivo:** gravar e ler agregados com segurança.

- Serviço de proteção de CPF (AES-GCM e HMAC-SHA256)
- Gerador de matrícula com transação
- `IAlunoRepository` e `IResponsavelRepository` no Domain
- Repositórios na Infrastructure
- Testes de integração dos repositórios

**Critério de conclusão:** aluno com responsáveis é gravado e consultado sem expor o CPF.

## Etapa 2: Application ⏳

**Objetivo:** casos de uso do negócio.

- Cadastrar, editar, consultar, listar e inativar aluno
- Cadastrar e consultar responsável
- DTOs de entrada e saída
- Validação com FluentValidation
- Testes unitários com mocks

**Critério de conclusão:** casos de uso testados sem banco.

## Etapa 3: API ⏳

**Objetivo:** expor os casos de uso via REST.

- Controllers de alunos e responsáveis
- Tratamento de erros com `ProblemDetails`
- Documentação OpenAPI
- Teste de arquitetura com NetArchTest

**Critério de conclusão:** endpoints respondem com os status HTTP corretos.

## Etapa 4: Autenticação ⏳

**Objetivo:** proteger a API.

- Cadastro de usuário e login
- Senha com BCrypt
- Emissão e validação de JWT
- Autorização nos endpoints

**Critério de conclusão:** endpoints protegidos retornam `401` sem token.

## Etapa 5: Front-end ⏳

**Objetivo:** interface de cadastro e consulta.

- Projeto React com TypeScript e Vite
- Login
- Listagem, cadastro e edição de alunos
- Vínculo de responsáveis
- TanStack Query para acesso à API

**Critério de conclusão:** fluxo completo funcionando no navegador.

## Etapa 6: Automação ⏳

**Objetivo:** execução e validação reproduzíveis.

- Dockerfile da API
- Docker Compose completo (API, banco e front)
- GitHub Actions com build e testes

**Critério de conclusão:** um push executa build e testes automaticamente.

## Etapa 7: Publicação ⏳

**Objetivo:** disponibilizar o projeto como portfólio.

- Banco PostgreSQL em nuvem
- Deploy da API e do front
- Segredos em variáveis de ambiente do provedor
- README final com link, prints e arquitetura

**Critério de conclusão:** aplicação acessível por URL pública, sem dados reais.