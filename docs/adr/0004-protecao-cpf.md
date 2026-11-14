# ADR-0004: Proteção de CPF

- **Status:** Aceita
- **Data:** 2026-11-14

## Contexto

O CPF é dado pessoal. Ele precisa ser protegido no banco e ainda permitir busca e garantia de unicidade.

## Decisão

Armazenar o CPF em duas colunas: `cpf_cifrado` (AES-GCM) e `cpf_hash` (HMAC-SHA256, 32 bytes). A API exibe o CPF apenas mascarado. A proteção é feita na Infrastructure, e o Domain só valida o CPF.

## Alternativas consideradas

- **CPF em texto puro:** simples, mas expõe o dado.
- **Somente criptografia:** não permite busca nem unicidade, pois o valor cifrado varia.
- **Somente hash:** permite busca, mas não permite exibir o valor.

## Consequências

- **Positivas:** busca e unicidade sem expor o CPF.
- **Negativas:** exige gerenciar duas chaves e dificulta a rotação delas.