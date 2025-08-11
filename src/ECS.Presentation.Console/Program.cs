using ECS.Application;
using ECS.Infrastructure;
using ECS.Domain;

// Manual wire-up (no DI to keep dependencies minimal)
var employeeRepo = new InMemoryEmployeeRepository();
var toolRepo = new InMemoryToolRepository();
var txRepo = new InMemoryTransactionRepository();

IAuthService auth = new AuthService(employeeRepo);
IToolService tools = new ToolService(toolRepo);
ITransactionService tx = new TransactionService(toolRepo, txRepo);
IReportingService reports = new ReportingService(toolRepo);

Console.WriteLine("ECS Skeleton Running (Week 5) — Architectural Framework");
Console.WriteLine("Logging in as demo/demo ...");
var user = auth.Authenticate("demo", "demo");
Console.WriteLine($"Authenticated: {user.Name} ({user.Username})");

var tool = tools.GetToolByBarcode("TL-0001");
Console.WriteLine($"Scanned: {tool.Barcode} (condition={tool.Condition}, custodian={tool.CurrentEmployeeId?.ToString() ?? "none"})");

Console.WriteLine("Checking out tool...");
var checkout = tx.Checkout(tool.Id, user.Id, "Looks good");
Console.WriteLine($"Checkout TX: {checkout.Id} at {checkout.TimestampUtc:u}");

Console.WriteLine("My tools:");
foreach (var t in tools.ListMyTools(user.Id))
{
    Console.WriteLine($" - {t.Barcode} (custodian={t.CurrentEmployeeId})");
}

Console.WriteLine("Checking in tool...");
var checkin = tx.Checkin(tool.Id, user.Id, "Returned OK");
Console.WriteLine($"Check-in TX: {checkin.Id} at {checkin.TimestampUtc:u}");

Console.WriteLine("Done. (This is a minimal flow to demonstrate architecture.)");
