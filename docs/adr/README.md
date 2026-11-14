# Decisões de Arquitetura (ADRs)

Cada ADR registra uma decisão importante e o motivo dela.
O que existe hoje está descrito em [architecture.md](../architecture.md), e o que ainda será feito está no [roadmap](../roadmap.md).

## Índice

| ADR | Título | Status |
|---|---|---|
| [0001](0001-clean-architecture-monolito.md) | Clean Architecture em monólito | Aceita |
| [0002](0002-estrutura-de-pastas.md) | Estrutura de pastas | Aceita |
| [0003](0003-autenticacao-jwt.md) | Autenticação com JWT | Aceita |
| [0004](0004-protecao-de-cpf.md) | Proteção de CPF | Aceita |
| [0005](0005-matricula-por-contador.md) | Matrícula por tabela de contadores | Aceita |
| [0006](0006-inativacao-logica.md) | Inativação lógica | Aceita |
| [0007](0007-schema-por-scripts-sql.md) | Banco por scripts SQL, sem migrations | Aceita |
| [0008](0008-modelos-de-persistencia-separados.md) | Modelos de persistência separados do Domain | Aceita |

## Como criar um ADR

1. Copie [0000-template.md](0000-template.md).
2. Nomeie o arquivo com o próximo número e um título curto, por exemplo `0009-titulo-da-decisao.md`.
3. Preencha contexto, decisão, alternativas e consequências.
4. Defina o status como `Proposta` até a decisão ser aceita.
5. Adicione o ADR ao índice acima.

## Status

- **Proposta:** em discussão.
- **Aceita:** decisão em vigor.
- **Substituída:** trocada por outro ADR, que deve ser citado no texto.

## Regras

- Não apague um ADR antigo. Se a decisão mudar, crie um novo e marque o anterior como `Substituída`.
- Cada ADR trata de uma única decisão.
- Os ADRs explicam o motivo. A descrição do estado atual fica em `architecture.md`.