# ADR-0008: Modelos de persistência separados do Domain

- **Status:** Aceita
- **Data:** 2026-11-14

## Contexto

O banco guarda o CPF em duas colunas e a matrícula em três, mas o Domain usa os Value Objects `Cpf` e `Matricula`.

## Decisão

Criar modelos de persistência (`AlunoPersistence`, `ResponsavelPersistence`, `AlunoResponsavelPersistence`) na Infrastructure, com mapeamento Fluent API. Os repositórios convertem entre esses modelos e as entidades do Domain.

## Alternativas consideradas

- **Mapear as entidades do Domain diretamente:** menos código, mas expõe o Domain a detalhes de persistência e de segurança.

## Consequências

- **Positivas:** o Domain não depende do EF Core nem da criptografia.
- **Negativas:** exige código de conversão nos repositórios.