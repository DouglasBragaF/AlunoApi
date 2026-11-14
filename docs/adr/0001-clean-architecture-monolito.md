# ADR-0001: Clean Architecture em monólito

- **Status:** Aceita
- **Data:** 2026-11-14

## Contexto

O projeto tem um domínio pequeno, um único contexto e será publicado como portfólio. Precisa ser simples de executar e permitir crescimento organizado.

## Decisão

Adotar um monólito em Clean Architecture, com as camadas Domain, Application, Infrastructure e Api. As dependências apontam sempre para o Domain.

## Alternativas consideradas

- **Microsserviços:** exigem mais infraestrutura e não trazem ganho para um domínio pequeno.
- **Camadas sem regra de dependência:** mais rápido no início, mas mistura regra de negócio com acesso a dados.

## Consequências

- **Positivas:** regras de negócio isoladas e testáveis, e evolução organizada.
- **Negativas:** mais projetos e mais código de mapeamento que uma API simples.