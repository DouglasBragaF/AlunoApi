BEGIN;

CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TABLE public.usuarios (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    email VARCHAR(200) NOT NULL,
    senha_hash VARCHAR(255) NOT NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    criado_em TIMESTAMPTZ NOT NULL DEFAULT NOW(),

    CONSTRAINT ck_usuarios_email
        CHECK (
            length(trim(email)) BETWEEN 5 AND 200
            AND position('@' IN trim(email)) > 1
        )
);

CREATE UNIQUE INDEX uq_usuarios_email
    ON public.usuarios (lower(trim(email)));


CREATE TABLE public.alunos (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(200) NOT NULL,
    data_nascimento DATE NOT NULL,

    cpf_cifrado BYTEA NOT NULL,
    cpf_hash BYTEA NOT NULL,

    rg VARCHAR(14),

    matricula_ano SMALLINT NOT NULL,
    matricula_sequencia INTEGER NOT NULL,
    matricula VARCHAR(10) NOT NULL,

    status SMALLINT NOT NULL DEFAULT 1,

    CONSTRAINT uq_alunos_cpf_hash
        UNIQUE (cpf_hash),

    CONSTRAINT uq_alunos_matricula
        UNIQUE (matricula),

    CONSTRAINT uq_alunos_ano_sequencia
        UNIQUE (matricula_ano, matricula_sequencia),

    CONSTRAINT ck_alunos_nome
        CHECK (length(trim(nome)) > 0),

    CONSTRAINT ck_alunos_cpf_hash
        CHECK (octet_length(cpf_hash) = 32),

    CONSTRAINT ck_alunos_status
        CHECK (status IN (1, 2)),

    CONSTRAINT ck_alunos_matricula_ano
        CHECK (matricula_ano BETWEEN 2000 AND 2999),

    CONSTRAINT ck_alunos_matricula_sequencia
        CHECK (matricula_sequencia BETWEEN 1 AND 999999),

    CONSTRAINT ck_alunos_matricula_formato
        CHECK (
            matricula = (
                matricula_ano::TEXT ||
                lpad(matricula_sequencia::TEXT, 6, '0')
            )
        )
);


CREATE TABLE public.responsaveis (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    nome VARCHAR(200) NOT NULL,

    cpf_cifrado BYTEA NOT NULL,
    cpf_hash BYTEA NOT NULL,

    CONSTRAINT uq_responsaveis_cpf_hash
        UNIQUE (cpf_hash),

    CONSTRAINT ck_responsaveis_nome
        CHECK (length(trim(nome)) > 0),

    CONSTRAINT ck_responsaveis_cpf_hash
        CHECK (octet_length(cpf_hash) = 32)
);


CREATE TABLE public.alunos_responsaveis (
    aluno_id UUID NOT NULL,
    responsavel_id UUID NOT NULL,

    CONSTRAINT pk_alunos_responsaveis
        PRIMARY KEY (aluno_id, responsavel_id),

    CONSTRAINT fk_alunos_responsaveis_aluno
        FOREIGN KEY (aluno_id)
        REFERENCES public.alunos (id)
        ON DELETE CASCADE,

    CONSTRAINT fk_alunos_responsaveis_responsavel
        FOREIGN KEY (responsavel_id)
        REFERENCES public.responsaveis (id)
        ON DELETE CASCADE
);


CREATE TABLE public.matricula_contadores (
    ano SMALLINT PRIMARY KEY,
    ultima_sequencia INTEGER NOT NULL DEFAULT 0,

    CONSTRAINT ck_matricula_contadores_ano
        CHECK (ano BETWEEN 2000 AND 2999),

    CONSTRAINT ck_matricula_contadores_sequencia
        CHECK (ultima_sequencia BETWEEN 0 AND 999999)
);


CREATE INDEX ix_alunos_nome
    ON public.alunos (nome);

CREATE INDEX ix_alunos_status
    ON public.alunos (status);

CREATE INDEX ix_alunos_data_nascimento
    ON public.alunos (data_nascimento);

CREATE INDEX ix_alunos_responsaveis_responsavel_id
    ON public.alunos_responsaveis (responsavel_id);

COMMIT;