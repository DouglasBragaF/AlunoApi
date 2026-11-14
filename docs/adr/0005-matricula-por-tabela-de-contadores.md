# ADR-0005: Matrícula por tabela de contadores

- **Status:** Aceita
- **Data:** 2026-11-14

## Contexto

A matrícula tem o formato `AAAA` + 6 dígitos, e a sequência reinicia a cada ano.

## Decisão

Usar a tabela `matricula_contadores`, com uma linha por ano, atualizada dentro de uma transação. As colunas `matricula_ano` e `matricula_sequencia` são únicas em conjunto.

## Alternativas consideradas

- **`MAX + 1`:** gera duplicidade em requisições simultâneas.
- **Sequence do PostgreSQL:** não reinicia por ano de forma simples.

## Consequências

- **Positivas:** sequência por ano, sem duplicidade.
- **Negativas:** exige transação e atenção ao bloqueio da linha do contador.