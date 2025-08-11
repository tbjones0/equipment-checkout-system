namespace ECS.Domain;

public class Supply
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;
    public Guid WarehouseId { get; set; }
}
