namespace ECS.Application;

public interface IScannerService
{
    /// <summary>Blocks until a barcode value is read or the user cancels (returns null).</summary>
    Task<string?> ReadAsync(CancellationToken ct = default);
}
