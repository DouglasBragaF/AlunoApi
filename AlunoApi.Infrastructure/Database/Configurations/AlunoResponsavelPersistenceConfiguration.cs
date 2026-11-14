using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AlunoResponsavelPersistenceConfiguration
    : IEntityTypeConfiguration<AlunoResponsavelPersistence>
{
    public void Configure(
        EntityTypeBuilder<AlunoResponsavelPersistence> builder)
    {
        builder.ToTable("alunos_responsaveis", "public");

        builder.HasKey(x => new
        {
            x.AlunoId,
            x.ResponsavelId
        });

        builder.Property(x => x.AlunoId)
            .HasColumnName("aluno_id")
            .HasColumnType("uuid");

        builder.Property(x => x.ResponsavelId)
            .HasColumnName("responsavel_id")
            .HasColumnType("uuid");

        builder.HasOne(x => x.Aluno)
            .WithMany(x => x.Responsaveis)
            .HasForeignKey(x => x.AlunoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Responsavel)
            .WithMany(x => x.Alunos)
            .HasForeignKey(x => x.ResponsavelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ResponsavelId)
            .HasDatabaseName("ix_alunos_responsaveis_responsavel_id");
    }
}