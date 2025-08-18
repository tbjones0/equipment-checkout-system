using ECS.Application;
using ECS.Infrastructure;
using ECS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// Config
var useSqlite = Environment.GetEnvironmentVariable("ECS_USE_SQLITE") == "1";
var connStr = Environment.GetEnvironmentVariable("ECS_SQLITE_DB") ?? "Data Source=ecs.db";

// Logging
using var loggerFactory = LoggerFactory.Create(b => b
    .AddSimpleConsole(o => { o.SingleLine = true; })
    .SetMinimumLevel(LogLevel.Information));

var logger = loggerFactory.CreateLogger("ECS");

IEmployeeRepository employeeRepo;
IToolRepository toolRepo;
ITransactionRepository txRepo;
IScannerService scanner = new ConsoleScannerAdapter();

if (useSqlite)
{
    logger.LogInformation("Starting with SQLite at {Conn}", connStr);

    var options = new DbContextOptionsBuilder<EcsDbContext>()
        .UseSqlite(connStr)
        .Options;

    // NOTE: do NOT use 'using' here — we keep the context alive for the whole app.
    var db = new EcsDbContext(options);
    db.Database.EnsureCreated();

    // Seed demo data if empty
    if (!db.Employees.Any())
    {
        db.Employees.Add(new Employee { Name = "Demo User", Username = "demo", PasswordHash = "demo" });
        db.Tools.Add(new Tool { Barcode = "TL-0001", Condition = "OK" });
        db.SaveChanges();
    }

    employeeRepo = new EfEmployeeRepository(db);
    toolRepo     = new EfToolRepository(db);
    txRepo       = new EfTransactionRepository(db);

    // Dispose the DbContext when the process exits (optional)
    AppDomain.CurrentDomain.ProcessExit += (_, __) => db.Dispose();
}
else
{
    logger.LogWarning("Starting with InMemory repositories (set ECS_USE_SQLITE=1 to enable SQLite).");
    employeeRepo = new InMemoryEmployeeRepository();
    toolRepo     = new InMemoryToolRepository();
    txRepo       = new InMemoryTransactionRepository();
}

// Services
IAuthService auth         = new AuthService(employeeRepo);
IToolService tools        = new ToolService(toolRepo);
ITransactionService tx    = new TransactionService(toolRepo, txRepo);
IReportingService reports = new ReportingService(toolRepo);

Console.WriteLine("ECS Week 6 — Demo Flow");
Console.WriteLine("Logging in as demo/demo ...");
var user = auth.Authenticate("demo", "demo");
Console.WriteLine($"Authenticated: {user.Name} ({user.Username})");

// Scan or use default
var code = await scanner.ReadAsync() ?? "TL-0001";
logger.LogInformation("Scanned: {Code}", code);
var tool = toolRepo.GetByBarcode(code) ?? throw new UnknownBarcodeException();

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

Console.WriteLine("Done.");
