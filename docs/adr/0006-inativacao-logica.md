# ADR-0006: Inativação lógica

- **Status:** Aceita
- **Data:** 2026-11-14

## Contexto

O histórico do aluno deve ser preservado.

## Decisão

Não excluir fisicamente. O aluno é inativado pelo campo `status` (1 = Ativo, 2 = Inativo), e aluno inativo não pode ser alterado.

## Alternativas consideradas

- **Exclusão física:** perde o histórico.

## Consequências

- **Positivas:** histórico preservado e regra no Domain.
- **Negativas:** consultas precisam filtrar por `status`.