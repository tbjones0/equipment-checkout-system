# ECS — Architectural Framework (Week 5)

This repository contains a minimal architectural skeleton for the Equipment Checkout System (ECS),
aligned with the SDD (v1.3). It compiles and runs as a console app and demonstrates a simple scan/checkout/check-in flow using in-memory data.

## Projects
- **ECS.Domain** — Entities, enums, domain errors
- **ECS.Application** — Service & repository interfaces
- **ECS.Infrastructure** — In-memory repositories and service implementations
- **ECS.Presentation.Console** — Console app with a demo flow

## Build & Run (Visual Studio / .NET 8)
1. Open **Visual Studio** → **Open a project or solution** → select the `src` folder (or create a solution and add the four projects).
2. Set **ECS.Presentation.Console** as Startup Project.
3. Build & Run (F5).

> Credentials: `demo` / `demo` (for demo AuthService only).

## Next steps (Week 6)
- Swap InMemory repositories for a real DB implementation
- Add barcode scanner adapters and UI
- Extend error handling & logging per SDD
