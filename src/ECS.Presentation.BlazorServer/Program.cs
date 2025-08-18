using ECS.Application;
using ECS.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("EcsSqlite") ?? "Data Source=ecs.db";
builder.Services.AddDbContext<EcsDbContext>(o => o.UseSqlite(conn));

builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();
builder.Services.AddScoped<IToolRepository, EfToolRepository>();
builder.Services.AddScoped<ITransactionRepository, EfTransactionRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IToolService, ToolService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IReportingService, ReportingService>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// DB init/seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EcsDbContext>();
    db.Database.EnsureCreated();
    if (!db.Employees.Any())
    {
        db.Employees.Add(new ECS.Domain.Employee { Name = "Demo User", Username = "demo", PasswordHash = "demo" });
        db.Tools.Add(new ECS.Domain.Tool { Barcode = "TL-0001", Condition = "OK" });
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
