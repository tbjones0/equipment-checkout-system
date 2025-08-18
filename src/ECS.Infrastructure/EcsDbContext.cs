using ECS.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECS.Infrastructure;

public class EcsDbContext : DbContext
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Tool> Tools => Set<Tool>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public EcsDbContext(DbContextOptions<EcsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Employee>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Username).IsUnique();
            e.Property(x => x.Name).HasMaxLength(100);
            e.Property(x => x.PasswordHash).HasMaxLength(128);
            e.Property(x => x.Role).HasMaxLength(50);
        });

        b.Entity<Tool>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Barcode).IsUnique();
            e.Property(x => x.Barcode).HasMaxLength(64);
            e.Property(x => x.Condition).HasMaxLength(64);
        });

        b.Entity<Transaction>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.TimestampUtc);
            e.Property(x => x.ConditionNote).HasMaxLength(256);
            e.HasIndex(x => x.EmployeeId);
            e.HasIndex(x => x.ToolId);
        });
    }
}
