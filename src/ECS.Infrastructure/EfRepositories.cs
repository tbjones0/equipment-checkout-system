using ECS.Application;
using ECS.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECS.Infrastructure;

public class EfEmployeeRepository : IEmployeeRepository
{
    private readonly EcsDbContext _db;
    public EfEmployeeRepository(EcsDbContext db) => _db = db;

    public Employee? GetByUsername(string username) =>
        _db.Employees.AsNoTracking().FirstOrDefault(e => e.Username == username);

    public Employee Upsert(Employee e)
    {
        var existing = _db.Employees.FirstOrDefault(x => x.Id == e.Id);
        if (existing is null) _db.Employees.Add(e);
        else _db.Entry(existing).CurrentValues.SetValues(e);
        _db.SaveChanges();
        return e;
    }
}

public class EfToolRepository : IToolRepository
{
    private readonly EcsDbContext _db;
    public EfToolRepository(EcsDbContext db) => _db = db;

    public Tool? GetByBarcode(string barcode) =>
        _db.Tools.FirstOrDefault(t => t.Barcode == barcode);

    public Tool? GetById(Guid id) => _db.Tools.Find(id);

    public Tool Upsert(Tool tool)
    {
        var existing = _db.Tools.FirstOrDefault(x => x.Id == tool.Id);
        if (existing is null) _db.Tools.Add(tool);
        else _db.Entry(existing).CurrentValues.SetValues(tool);
        _db.SaveChanges();
        return tool;
    }

    public IEnumerable<Tool> ListByEmployee(Guid employeeId) =>
        _db.Tools.AsNoTracking().Where(t => t.CurrentEmployeeId == employeeId).ToList();
}

public class EfTransactionRepository : ITransactionRepository
{
    private readonly EcsDbContext _db;
    public EfTransactionRepository(EcsDbContext db) => _db = db;

    public Transaction Add(Transaction t)
    {
        _db.Transactions.Add(t);
        _db.SaveChanges();
        return t;
    }

    public IEnumerable<Transaction> ListByTool(Guid toolId) =>
        _db.Transactions.AsNoTracking().Where(tx => tx.ToolId == toolId).ToList();

    public IEnumerable<Transaction> ListByEmployee(Guid employeeId) =>
        _db.Transactions.AsNoTracking().Where(tx => tx.EmployeeId == employeeId).ToList();
}
