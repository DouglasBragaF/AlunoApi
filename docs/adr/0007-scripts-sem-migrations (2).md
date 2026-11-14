# ADR-0007: Banco por scripts SQL, sem migrations

- **Status:** Aceita
- **Data:** 2026-11-14

## Contexto

O fluxo de migrations do EF Core foi considerado custoso para este projeto.

## Decisão

Criar e evoluir o schema com scripts SQL numerados, em `AlunoApi.Infrastructure/Database`. O Docker Compose executa os scripts na primeira criação do volume. O EF Core apenas mapeia o schema existente, sem `Migrate()` nem `EnsureCreated()`.

## Alternativas consideradas

- **Migrations do EF Core:** automáticas, mas custosas neste contexto.
- **`EnsureCreated()`:** não versiona alterações.

## Consequências

- **Positivas:** controle total do SQL e schema documentado no repositório.
- **Negativas:** o mapeamento e o script precisam ser mantidos em sincronia. Os testes de integração protegem essa regra.