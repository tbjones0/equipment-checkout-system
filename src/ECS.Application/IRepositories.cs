using ECS.Domain;

namespace ECS.Application;

public interface IEmployeeRepository
{
    Employee? GetByUsername(string username);
    Employee Upsert(Employee e);
}

public interface IToolRepository
{
    Tool? GetByBarcode(string barcode);
    Tool? GetById(Guid id);
    Tool Upsert(Tool tool);
    IEnumerable<Tool> ListByEmployee(Guid employeeId);
}

public interface ITransactionRepository
{
    Transaction Add(Transaction t);
    IEnumerable<Transaction> ListByTool(Guid toolId);
    IEnumerable<Transaction> ListByEmployee(Guid employeeId);
}
