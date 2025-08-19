# ECS — Interactive Equipment Checkout System (Week 6)

This repository contains the Equipment Checkout System (ECS) implementation for Team 9 - DeVry University.
The system now features a **fully interactive user interface** that demonstrates all 5 required use cases with real user interaction.

---

## ✨ Key Features
- **User Authentication**: Interactive login system (demo/demo)
- **Equipment Checkout**: User-driven barcode scanning and checkout process
- **Equipment Check-in**: Interactive tool return functionality  
- **Tool Management**: View personal checked-out tools
- **Overdue Tracking**: Monitor and display overdue equipment
- **Material Requests**: Submit requests for additional materials
- **SQLite Database**: Persistent data storage with automatic seeding
- **Clean Architecture**: Domain, Application, Infrastructure, and Presentation layers

---

## 📂 Project Structure
- **ECS.Domain** — Entities, enums, domain errors, business rules
- **ECS.Application** — Service & repository interfaces, business logic  
- **ECS.Infrastructure** — EF Core (SQLite) repositories and service implementations  
- **ECS.Presentation.Console** — Interactive console application with menu system
- **ECS.Presentation.BlazorServer** — Web UI (starter shell for future expansion)

---

## 🚀 Quick Start

### Option 1: Run from Any Directory (Recommended)
```bash
# Clone the repository
git clone https://github.com/tbjones0/equipment-checkout-system.git

# Run from anywhere using the --project flag
dotnet run --project equipment-checkout-system/src/ECS.Presentation.Console
```

### Option 2: Use the Run Script (Even Easier)
```bash
# Clone the repository
git clone https://github.com/tbjones0/equipment-checkout-system.git
cd equipment-checkout-system

# Make the script executable (Mac/Linux)
chmod +x run-ecs.sh

# Run the system
./run-ecs.sh
```

### Option 3: Traditional Method
```bash
cd equipment-checkout-system/src/ECS.Presentation.Console
dotnet run
```

---

## 🎮 Using the Interactive System

### Login
- Username: `demo`
- Password: `demo`

### Main Menu Options
1. **Check OUT equipment** - Enter tool barcode (e.g., `TL-0001`)
2. **Check IN equipment** - Return tools by barcode
3. **View my tools** - See your currently checked-out equipment
4. **View overdue items** - Display tools overdue for return
5. **Request materials** - Submit material requests
0. **Exit** - Close the application

### Sample Data
- Tool Barcode: `TL-0001` (automatically seeded in database)
- Demo User: `demo` with password `demo`

---

## 🛠 Build Instructions

### Prerequisites
- .NET 8 SDK
- Git (for cloning)

### Build Commands
```bash
# Clean previous build artifacts
dotnet clean

# Build the solution
dotnet build

# Run tests (if implemented)
dotnet test
```

---

## 🐞 Debug in Visual Studio Code

1. Ensure the `.vscode/launch.json` file is present
2. Open the **Run and Debug** tab (▶️ with bug icon)
3. Select **".NET Core Launch (ECS Console)"**
4. Press **F5** to start debugging

---

## 🗄 Database

The system uses **SQLite** for persistent storage:
- Database file: `ecs.db` (auto-created in the console project directory)
- **Environment Variables**:
  - `ECS_USE_SQLITE=1` (enables SQLite, set by default)
  - `ECS_SQLITE_DB` (custom database path, optional)

### Demo Data
The system automatically seeds demo data on first run:
- Demo user account (`demo`/`demo`)
- Sample tool (`TL-0001`)

---

## 🧪 Testing the 5 Use Cases

1. **User Authentication** - System prompts for login credentials
2. **Equipment Checkout** - Select option 1, enter barcode `TL-0001`
3. **View Personal Tools** - Select option 3 to see checked-out items
4. **Equipment Check-in** - Select option 2, enter barcode to return
5. **Overdue Management** - Select option 4 to view overdue items
6. **Material Requests** - Select option 5 to request materials

**Note**: The system now requires **real user interaction** - no automated scripting or demo modes unless explicitly requested with `--demo` flag.

---

## 🌐 Web Interface (Future)

A Blazor Server web interface is available but currently in development:
```bash
dotnet run --project src/ECS.Presentation.BlazorServer
```
Visit `http://localhost:5000` (or shown URL) for the web interface.

---

## 👥 Team 9 Members
- Timothy Jones
- Tyler Feret  
- Angel Lively
- Harold Byrd

---

## 📝 Documentation
- Test cases: `/docs/ECS_Week6_TestCases.docx`
- Software Design Document: Available in `/docs/`
- Architecture follows Clean Architecture principles with clear separation of concerns
