using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ResponsavelPersistenceConfiguration
    : IEntityTypeConfiguration<ResponsavelPersistence>
{
    public void Configure(EntityTypeBuilder<ResponsavelPersistence> builder)
    {
        builder.ToTable("responsaveis", "public");

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

        builder.Property(x => x.CpfCifrado)
            .HasColumnName("cpf_cifrado")
            .HasColumnType("bytea")
            .IsRequired();

        builder.Property(x => x.CpfHash)
            .HasColumnName("cpf_hash")
            .HasColumnType("bytea")
            .HasMaxLength(32)
            .IsRequired();

        builder.HasIndex(x => x.CpfHash)
            .IsUnique()
            .HasDatabaseName("uq_responsaveis_cpf_hash");

        builder.HasIndex(x => x.Nome)
            .HasDatabaseName("ix_responsaveis_nome");
    }
}