using ECS.Application;
using ECS.Domain;

namespace ECS.Infrastructure;

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly Dictionary<string, Employee> _byUser = new(StringComparer.OrdinalIgnoreCase);

    public InMemoryEmployeeRepository()
    {
        // seed demo user
        var e = new Employee { Name = "Demo User", Username = "demo", PasswordHash = "demo", Role = "Employee" };
        _byUser[e.Username] = e;
    }

    public Employee? GetByUsername(string username) => _byUser.TryGetValue(username, out var e) ? e : null;

    public Employee Upsert(Employee e)
    {
        _byUser[e.Username] = e;
        return e;
    }
}

public class InMemoryToolRepository : IToolRepository
{
    private readonly Dictionary<Guid, Tool> _byId = new();
    private readonly Dictionary<string, Guid> _byBarcode = new(StringComparer.OrdinalIgnoreCase);

    public InMemoryToolRepository()
    {
        // seed 2 tools
        var t1 = new Tool { Barcode = "TL-0001", Condition = "OK" };
        var t2 = new Tool { Barcode = "TL-0002", Condition = "OK" };
        _byId[t1.Id] = t1; _byBarcode[t1.Barcode] = t1.Id;
        _byId[t2.Id] = t2; _byBarcode[t2.Barcode] = t2.Id;
    }

    public Tool? GetByBarcode(string barcode)
        => _byBarcode.TryGetValue(barcode, out var id) && _byId.TryGetValue(id, out var t) ? t : null;

    public Tool? GetById(Guid id) => _byId.TryGetValue(id, out var t) ? t : null;

    public Tool Upsert(Tool tool)
    {
        _byId[tool.Id] = tool;
        _byBarcode[tool.Barcode] = tool.Id;
        return tool;
    }

    public IEnumerable<Tool> ListByEmployee(Guid employeeId) => _byId.Values.Where(t => t.CurrentEmployeeId == employeeId);
}

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly List<Transaction> _tx = new();

    public Transaction Add(Transaction t)
    {
        _tx.Add(t);
        return t;
    }

    public IEnumerable<Transaction> ListByTool(Guid toolId) => _tx.Where(x => x.ToolId == toolId);

    public IEnumerable<Transaction> ListByEmployee(Guid employeeId) => _tx.Where(x => x.EmployeeId == employeeId);
}
