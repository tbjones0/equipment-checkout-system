using ECS.Application;
using ECS.Domain;

namespace ECS.Infrastructure;

public class AuthService : IAuthService
{
    private readonly IEmployeeRepository _employees;
    public AuthService(IEmployeeRepository employees) => _employees = employees;

    public Employee Authenticate(string username, string secret)
    {
        var e = _employees.GetByUsername(username) ?? throw new DomainException("Unknown user.");
        // DEMO ONLY: compare to PasswordHash field (not secure)
        if (e.PasswordHash != secret) throw new DomainException("Invalid credentials.");
        return e;
    }
}

public class ToolService : IToolService
{
    private readonly IToolRepository _tools;
    public ToolService(IToolRepository tools) => _tools = tools;

    public Tool GetToolByBarcode(string barcode)
        => _tools.GetByBarcode(barcode) ?? throw new UnknownBarcodeException();

    public Tool? GetById(Guid id) => _tools.GetById(id);

    public void UpdateCondition(Guid toolId, string condition)
    {
        var t = _tools.GetById(toolId) ?? throw new DomainException("Unknown tool.");
        t.Condition = condition;
        _tools.Upsert(t);
    }

    public IEnumerable<Tool> ListMyTools(Guid employeeId) => _tools.ListByEmployee(employeeId);
}

public class TransactionService : ITransactionService
{
    private readonly IToolRepository _tools;
    private readonly ITransactionRepository _tx;

    public TransactionService(IToolRepository tools, ITransactionRepository tx)
    {
        _tools = tools;
        _tx = tx;
    }

    public Transaction Checkout(Guid toolId, Guid employeeId, string? conditionNote = null)
    {
        var tool = _tools.GetById(toolId) ?? throw new DomainException("Unknown tool.");
        if (tool.CurrentEmployeeId != null) throw new ToolNotAvailableException();
        tool.CurrentEmployeeId = employeeId;
        _tools.Upsert(tool);

        var t = new Transaction { ToolId = toolId, EmployeeId = employeeId, Action = TransactionAction.CheckOut, ConditionNote = conditionNote };
        return _tx.Add(t);
    }

    public Transaction Checkin(Guid toolId, Guid employeeId, string? conditionNote = null)
    {
        var tool = _tools.GetById(toolId) ?? throw new DomainException("Unknown tool.");
        if (tool.CurrentEmployeeId != employeeId) throw new NotCustodianException();
        tool.CurrentEmployeeId = null;
        _tools.Upsert(tool);

        var t = new Transaction { ToolId = toolId, EmployeeId = employeeId, Action = TransactionAction.CheckIn, ConditionNote = conditionNote };
        return _tx.Add(t);
    }
}

public class ReportingService : IReportingService
{
    private readonly IToolRepository _tools;
    public ReportingService(IToolRepository tools) => _tools = tools;

    public IEnumerable<Tool> ToolsByEmployee(Guid employeeId) => _tools.ListByEmployee(employeeId);
}
