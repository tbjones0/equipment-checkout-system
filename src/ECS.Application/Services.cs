using ECS.Domain;

namespace ECS.Application;

public interface IAuthService
{
    Employee Authenticate(string username, string secret);
}

public interface IToolService
{
    Tool GetToolByBarcode(string barcode);
    Tool? GetById(Guid id);
    void UpdateCondition(Guid toolId, string condition);
    IEnumerable<Tool> ListMyTools(Guid employeeId);
}

public interface ITransactionService
{
    Transaction Checkout(Guid toolId, Guid employeeId, string? conditionNote = null);
    Transaction Checkin(Guid toolId, Guid employeeId, string? conditionNote = null);
}

public interface IReportingService
{
    IEnumerable<Tool> ToolsByEmployee(Guid employeeId);
}
