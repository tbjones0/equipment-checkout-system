namespace ECS.Domain;

public class Employee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // placeholder for demo
    public string Role { get; set; } = "Employee";
}
