# ADR-0002: Estrutura de pastas

- **Status:** Aceita
- **Data:** 2026-11-14

## Contexto

É preciso um padrão simples para localizar código e documentação.

## Decisão

Manter um projeto por camada na raiz da solução, com o projeto de testes separado e a documentação em `docs/`. O Domain é organizado por agregado (`Alunos`, `Responsaveis`) e `ValueObjects`.

## Alternativas consideradas

- **Pastas `src/` e `tests/`:** mais comum em projetos grandes, mas desnecessário para cinco projetos.

## Consequências

- **Positivas:** estrutura direta e fácil de navegar.
- **Negativas:** pode exigir reorganização se o número de projetos crescer.