using Microsoft.EntityFrameworkCore;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AlunoPersistence> Alunos => Set<AlunoPersistence>();
    public DbSet<ResponsavelPersistence> Responsaveis => Set<ResponsavelPersistence>();
    public DbSet<AlunoResponsavelPersistence> AlunosResponsaveis =>
    Set<AlunoResponsavelPersistence>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}