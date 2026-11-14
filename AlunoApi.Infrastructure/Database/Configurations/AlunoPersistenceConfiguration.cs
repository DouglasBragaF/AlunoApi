using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AlunoPersistenceConfiguration
    : IEntityTypeConfiguration<AlunoPersistence>
{
    public void Configure(EntityTypeBuilder<AlunoPersistence> builder)
    {
        builder.ToTable("alunos", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.DataNascimento)
            .HasColumnName("data_nascimento")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.CpfCifrado)
            .HasColumnName("cpf_cifrado")
            .HasColumnType("bytea")
            .IsRequired();

        builder.Property(x => x.CpfHash)
            .HasColumnName("cpf_hash")
            .HasColumnType("bytea")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Rg)
            .HasColumnName("rg")
            .HasMaxLength(14);

        builder.Property(x => x.MatriculaAno)
            .HasColumnName("matricula_ano")
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(x => x.MatriculaSequencia)
            .HasColumnName("matricula_sequencia")
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(x => x.Matricula)
            .HasColumnName("matricula")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("smallint")
            .IsRequired();

        builder.HasIndex(x => x.CpfHash)
            .IsUnique()
            .HasDatabaseName("uq_alunos_cpf_hash");

        builder.HasIndex(x => x.Matricula)
            .IsUnique()
            .HasDatabaseName("uq_alunos_matricula");

        builder.HasIndex(x => new
        {
            x.MatriculaAno,
            x.MatriculaSequencia
        })
        .IsUnique()
        .HasDatabaseName("uq_alunos_ano_sequencia");

        builder.HasIndex(x => x.Nome)
            .HasDatabaseName("ix_alunos_nome");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("ix_alunos_status");
    }
}