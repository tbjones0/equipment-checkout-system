using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECS.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EcsDbContext>
{
    public EcsDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<EcsDbContext>()
            .UseSqlite("Data Source=ecs.db")
            .Options;
        return new EcsDbContext(options);
    }
}
