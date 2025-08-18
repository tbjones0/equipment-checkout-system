using ECS.Application;

namespace ECS.Infrastructure;

public class ConsoleScannerAdapter : IScannerService
{
    public Task<string?> ReadAsync(CancellationToken ct = default)
    {
        Console.Write("Scan barcode (or press Enter to cancel): ");
        var s = Console.ReadLine();
        return Task.FromResult(string.IsNullOrWhiteSpace(s) ? null : s.Trim());
    }
}
