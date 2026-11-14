# ADR-0003: Autenticação com JWT

- **Status:** Aceita
- **Data:** 2026-11-14

## Contexto

O front-end React precisa se autenticar em uma API sem estado.

## Decisão

Usar JWT (Bearer) para autenticação e BCrypt para o hash das senhas. As chaves ficam em variáveis de ambiente ou User Secrets.

## Alternativas consideradas

- **Sessão com cookie:** exige estado no servidor.
- **Provedor externo de identidade:** mais completo, mas aumenta a complexidade do MVP.

## Consequências

- **Positivas:** API sem estado e integração simples com o React.
- **Negativas:** é preciso tratar expiração e proteção do token no cliente.