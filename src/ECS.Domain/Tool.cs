namespace ECS.Domain;

public class Tool
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Barcode { get; set; } = string.Empty;
    public string Condition { get; set; } = "OK";
    // 0..1 current custodian
    public Guid? CurrentEmployeeId { get; set; } = null;
}
