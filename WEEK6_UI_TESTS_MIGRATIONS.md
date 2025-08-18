# Week 6 — UI, Tests & Migrations

## Add projects to the solution
```bash
dotnet sln add src/ECS.Presentation.BlazorServer/ECS.Presentation.BlazorServer.csproj
dotnet sln add tests/ECS.Tests/ECS.Tests.csproj
```

## Add EF tools (optional, for migrations)
```bash
dotnet tool install --global dotnet-ef
```

## Create migrations (SQLite)
```bash
# From the solution root:
dotnet ef migrations add InitialCreate -p src/ECS.Infrastructure -s src/ECS.Presentation.Console
dotnet ef database update -p src/ECS.Infrastructure -s src/ECS.Presentation.Console
```

> The Infrastructure project includes `DesignTimeDbContextFactory` so EF can create the context at design-time.

## Run Console (SQLite)
```bash
export ECS_USE_SQLITE=1
export ECS_SQLITE_DB=ecs.db
dotnet run --project src/ECS.Presentation.Console
```

## Run Blazor Server UI
```bash
dotnet run --project src/ECS.Presentation.BlazorServer
# Navigate to https://localhost:5001 or the URL shown in console
```

## Run Tests
```bash
dotnet test tests/ECS.Tests/ECS.Tests.csproj
```
