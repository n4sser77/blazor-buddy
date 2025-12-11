# BlazorBuddy

BlazorBuddy is a real-time collaborative study companion application built with Blazor Server and .NET 10. Users can write and categorize markdown notes, chat with other users and sketch on a canvas.

## Getting Started

### Prerequisites

* [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
* SQL Server LocalDB (optional, for default dev config)

### Cloning the Repository

This project uses Git Submodules. You must clone recursively or initialize submodules after cloning.

#### Option 1: Clone with submodules

```bash
git clone --recursive https://github.com/n4sser77/blazor-buddy.git
```

#### Option 2: Initialize submodules after cloning

If you already cloned without the submodules
You can run the command below to get and update the submodules

```bash
git clone https://github.com/n4sser77/blazor-buddy.git
cd blazor-buddy
git submodule update --init --recursive
```

### Running the Application

 Run the application:

   ```bash
   dotnet run --project BlazorBuddy.WebApp
   ```

      *By default, the app uses `(localdb)\mssqllocaldb`. Ensure you have it installed or update `appsettings.json` with your connection string.*

 Open your browser and navigate to `https://localhost:7000` (or the port shown in the terminal).

### Running Tests

The solution includes integration tests that verify the application flow using an in-memory database.

Run the tests:

```bash
   dotnet test
```

## System Architecture

The application follows a **Monolithic Layered Architecture** using **Blazor Server** for the presentation layer.

### High-Level Overview

* **BlazorBuddy.Core**: Contains the domain entities (Models) and shared logic. This layer has no dependencies on the UI or Data Access layers, ensuring high cohesion for domain logic.
* **BlazorBuddy.WebApp**: The main entry point. It hosts the Blazor Server application, manages Dependency Injection, and handles HTTP requests.
  * **Components**: Razor components for the UI.
  * **Services**: Business logic layer (e.g., `ChatService`, `NotesStateService`).
  * **Repositories**: Data access layer using Entity Framework Core.
* **BlazorBuddy.Test**: Integration tests project.

### Key Design Choices

* **Blazor Server**: Chosen for its ability to build rich, interactive UIs with C# without needing a separate frontend framework. It maintains a real-time connection via SignalR.
* **Dependency Injection (DI)**: Extensively used to decouple components from concrete implementations. Interfaces (e.g., `INoteRepo`, `IChatService`) are injected, promoting loose coupling and testability.
* **State Management**:
  * **Singleton Services**: Used for global state or caching (e.g., `NotesStateService` for caching notes, `ChatEventBroker` for broadcasting messages).
  * **Scoped Services**: Used for per-request/per-circuit operations (e.g., Repositories).
* **Entity Framework Core**: Used for ORM. The app is configured to use **SQL Server** in production/development and **InMemory** database for Integration Tests.

### Coupling and Cohesion

* **Cohesion**: The project is structured to keep related functionality together. `Services` handle business rules, while `Repositories` strictly handle data persistence. `Core` is pure domain.
* **Coupling**: Low coupling is achieved through the use of Interfaces. The WebApp depends on abstractions rather than concrete implementations where possible, allowing for easier unit testing and maintenance.

### Testability

The architecture is designed with testability in mind:

* **Integration Tests**: The `BlazorBuddy.Test` project uses `WebApplicationFactory` to spin up the app in-memory.

* **Database Abstraction**: The `CustomWebApplicationFactory` replaces the SQL Server provider with an InMemory database, allowing tests to run without external infrastructure dependencies.
* **Service Isolation**: Services can be unit tested by mocking their repository dependencies.

## Dependencies

* **Microsoft.AspNetCore.Identity.EntityFrameworkCore**: For user authentication and management.
* **Microsoft.EntityFrameworkCore.SqlServer**: Database provider.
* **PSC.Blazor.Components.MarkdownEditor**: A Markdown editor component (included as a submodule).

## Project Structure

```text
BlazorBuddy/
 BlazorBuddy.Core/       # Domain Models (POCOs)
 BlazorBuddy.WebApp/     # Main Blazor Server Application
    Components/         # UI Components (Pages, Layouts)
    Services/           # Business Logic & State
    Repositories/       # Data Access (EF Core)
    Data/               # EF DbContext & Migrations
    Program.cs          # App Entry & DI Configuration
 BlazorBuddy.Test/       # Integration Tests
 Libs/                   # External Libraries (Submodules)
```
