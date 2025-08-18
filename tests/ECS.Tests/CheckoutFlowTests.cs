using ECS.Application;
using ECS.Infrastructure;
using ECS.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class CheckoutFlowTests
{
    [Fact]
    public void Checkout_Then_Checkin_Persists_WithEfCore()
    {
        var options = new DbContextOptionsBuilder<EcsDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        using var db = new EcsDbContext(options);
        db.Database.OpenConnection();
        db.Database.EnsureCreated();

        var e = new Employee { Name = "Tester", Username = "u", PasswordHash = "p" };
        var t = new Tool { Barcode = "TL-TEST", Condition = "OK" };
        db.Employees.Add(e);
        db.Tools.Add(t);
        db.SaveChanges();

        var empRepo = new EfEmployeeRepository(db);
        var toolRepo = new EfToolRepository(db);
        var txRepo = new EfTransactionRepository(db);

        var svc = new TransactionService(toolRepo, txRepo);

        var outTx = svc.Checkout(t.Id, e.Id, "ok");
        Assert.NotEqual(Guid.Empty, outTx.Id);
        var refreshed = toolRepo.GetById(t.Id);
        Assert.Equal(e.Id, refreshed!.CurrentEmployeeId);

        var inTx = svc.Checkin(t.Id, e.Id, "back");
        Assert.NotEqual(Guid.Empty, inTx.Id);
        refreshed = toolRepo.GetById(t.Id);
        Assert.Null(refreshed!.CurrentEmployeeId);
    }
}
