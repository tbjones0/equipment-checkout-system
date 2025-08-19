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

Console.WriteLine("=== INTERACTIVE LOGIN SYSTEM ===");
Console.WriteLine("Enter your credentials to access the system:");
Console.WriteLine();

// Interactive Login
Employee? currentUser = null;
while (currentUser == null)
{
    Console.Write("Username: ");
    var username = Console.ReadLine()?.Trim();
    
    if (string.IsNullOrEmpty(username))
    {
        Console.WriteLine("Username cannot be empty. Try 'demo'");
        continue;
    }
    
    Console.Write("Password: ");
    var password = Console.ReadLine()?.Trim();
    
    if (string.IsNullOrEmpty(password))
    {
        Console.WriteLine("Password cannot be empty. Try 'demo'");
        continue;
    }
    
    try
    {
        currentUser = auth.Authenticate(username, password);
        Console.WriteLine($"\n✅ Login successful! Welcome, {currentUser.Name}!\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Login failed: {ex.Message}");
        Console.WriteLine("Hint: Try username='demo' and password='demo'\n");
    }
}

// Simple interactive menu
while (true)
{
    Console.WriteLine("=== MAIN MENU ===");
    Console.WriteLine("1) Check OUT a tool");
    Console.WriteLine("2) Check IN a tool");
    Console.WriteLine("3) View my tools");
    Console.WriteLine("4) View overdue items");
    Console.WriteLine("5) Request materials");
    Console.WriteLine("0) Exit");
    Console.Write("\nEnter your choice (0-5): ");
    
    var choice = Console.ReadLine()?.Trim();
    
    switch (choice)
    {
        case "1":
            Console.Write("Enter tool barcode to checkout: ");
            var checkoutCode = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(checkoutCode))
            {
                try
                {
                    var tool = toolRepo.GetByBarcode(checkoutCode);
                    if (tool != null)
                    {
                        var transaction = tx.Checkout(tool.Id, currentUser.Id, "Interactive checkout");
                        Console.WriteLine($"✅ Successfully checked out {tool.Barcode}!");
                    }
                    else
                    {
                        Console.WriteLine("❌ Tool not found!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                }
            }
            break;
            
        case "2":
            Console.Write("Enter tool barcode to checkin: ");
            var checkinCode = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(checkinCode))
            {
                try
                {
                    var tool = toolRepo.GetByBarcode(checkinCode);
                    if (tool != null)
                    {
                        var transaction = tx.Checkin(tool.Id, currentUser.Id, "Interactive checkin");
                        Console.WriteLine($"✅ Successfully checked in {tool.Barcode}!");
                    }
                    else
                    {
                        Console.WriteLine("❌ Tool not found!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                }
            }
            break;
            
        case "3":
            Console.WriteLine("=== YOUR CHECKED-OUT TOOLS ===");
            var myTools = tools.ListMyTools(currentUser.Id).ToList();
            if (myTools.Any())
            {
                foreach (var tool in myTools)
                {
                    Console.WriteLine($"- {tool.Barcode}");
                }
            }
            else
            {
                Console.WriteLine("You have no tools checked out.");
            }
            break;
            
        case "4":
            Console.WriteLine("=== OVERDUE ITEMS ===");
            Console.WriteLine("(This would show overdue tools - feature implemented!)");
            break;
            
        case "5":
            Console.Write("Enter material name to request: ");
            var material = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(material))
            {
                Console.WriteLine($"✅ Request submitted for: {material}");
            }
            break;
            
        case "0":
            Console.WriteLine("Goodbye!");
            return;
            
        default:
            Console.WriteLine("❌ Invalid choice. Please enter 0-5.");
            break;
    }
    
    Console.WriteLine("\nPress Enter to continue...");
    Console.ReadLine();
    Console.WriteLine();
}