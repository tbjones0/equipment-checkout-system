namespace ECS.Domain;

public class Warehouse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Location { get; set; } = string.Empty;
}
