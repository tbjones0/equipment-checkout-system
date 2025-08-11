namespace ECS.Domain;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmployeeId { get; set; }
    public Guid ToolId { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public TransactionAction Action { get; set; }
    public string? ConditionNote { get; set; }
}
