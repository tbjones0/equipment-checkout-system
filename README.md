# ECS — Architectural Framework (Week 5)

This repository contains a minimal architectural skeleton for the Equipment Checkout System (ECS),
aligned with the SDD (v1.3). It compiles and runs as a console app and demonstrates a simple scan/checkout/check-in flow using in-memory data.

---

## 📂 Projects
- **ECS.Domain** — Entities, enums, domain errors  
- **ECS.Application** — Service & repository interfaces  
- **ECS.Infrastructure** — In-memory repositories and service implementations  
- **ECS.Presentation.Console** — Console app with a demo flow  

---

## 🛠 Build & Run Instructions

### ▶ Visual Studio / .NET 8
1. Open **Visual Studio** → **Open a project or solution** → select the `src` folder (or create a solution and add the four projects).  
2. Set **ECS.Presentation.Console** as Startup Project.  
3. Build & Run (**F5**).  

---

### 💻 Build from the Command Line
From the project root:

```bash
# Clean previous build artifacts
dotnet clean equipment-checkout-system.sln

# Build the solution
dotnet build equipment-checkout-system.sln


### If the build succeeds, you’ll see:

```
Build succeeded.
0 Warning(s)
0 Error(s)
```

## 🚀 Run from the Command Line

To run the console application:

```bash
dotnet run --project src/ECS.Presentation.Console/ECS.Presentation.Console.csproj
```

## 🐞 Debug in Visual Studio Code

1. Ensure the `.vscode/launch.json` file is present (use the provided config).
2. In VS Code, open the **Run and Debug** tab (▶️ with a bug icon).
3. Select **“.NET Core Launch (ECS Console)”** from the dropdown.
4. Press **F5** to start debugging.

This will:
- Build the solution
- Launch `ECS.Presentation.Console.dll`
- Let you set breakpoints, inspect variables, and step through code

## 🔑 Demo Credentials

- Username: `demo`
- Password: `demo`  
*(for demo AuthService only)*

## 📌 Next Steps (Week 6)

- Swap InMemory repositories for a real DB implementation
- Add barcode scanner adapters and UI
- Extend error handling & logging per SDD
