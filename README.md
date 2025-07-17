# Infonetica Assignment: Workflow Service

This repository provides a Workflow Engine_ backend implemented as a .NET 8/C# Minimal API.

## Features

- **Workflow Definitions**: define named states and transitions (actions) between them.
- **Instance Management**: start workflow instances and move them through states via actions.
- **Validation**: enforce allowed transitions, enabled/disabled flags, and single initial state.
- **History Tracking**: each instance returns its current state and full action history.
- **Swagger UI**: built‑in OpenAPI docs for easy exploration.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)

## Build & Run

```bash
git clone https://github.com/Symbiot01/Infonetica_assignment.git
cd Infonetica_assignment/WorkflowService
dotnet restore
dotnet run
```

By default, the service listens on `http://localhost:5000`.  You can browse the Swagger UI at `http://localhost:5000/swagger`.

## Available Endpoints

| Method | Path                                            | Description                                  |
|--------|-------------------------------------------------|----------------------------------------------|
| **Swagger UI**  | `/swagger`                             | Explore and test all API routes              |
| `POST` | `/workflows`                                    | Create a new workflow definition             |
| `GET`  | `/workflows/{id}`                               | Retrieve a specific workflow definition      |
| `POST` | `/workflows/{id}/instances`                     | Start a new instance of a given definition   |
| `GET`  | `/instances/{id}`                               | Get an instance’s current state & history    |
| `POST` | `/instances/{id}/actions/{actionId}`            | Execute a transition action on an instance   |

> **Note:** This version does **not** expose listing all definitions or a separate history‑only route. That is for later implementing.

## Project Structure

```
Infonetica_assignment/
├── DTOs/                   # Data Transfer Objects (for incoming/outgoing payloads)
├── Exception/              # Custom exception types (e.g., ValidationException)
├── Models/                 # Domain types: State, ActionTransition, WorkflowDefinition, WorkflowInstance
├── Repositories/           # Interfaces and in-memory implementation
├── Services/               # WorkflowService with business logic
├── Validators/             # Definition and transition validation logic
├── Program.cs              # Minimal API endpoints & Swagger configuration
├── WorkflowService.csproj  # .NET project file
├── appsettings.json        # Default empty configuration
├── .gitignore              # Ignore rules
└── README.md               # Project overview and documentation
```

## Validation Rules

1. **Definitions** must have exactly one initial state and no duplicate IDs.
2. **Actions** must reference valid states and be enabled to execute.
3. **Transitions** are only allowed from the instance’s current state; final states reject further actions.
4. Attempting invalid operations returns HTTP 400 with a clear error message.

## Improvements & Next Steps

- Swap in a durable persistence layer (e.g., EF Core with SQL or document store) behind the repository interface for production readiness.
- Implement comprehensive unit and integration tests for validators, services, and API endpoints to ensure correctness.
- Add authentication/authorization (e.g., JWT bearer, role-based policies) to secure API access.
- Expose listing and query endpoints for workflows and instances, complete with pagination, sorting, and filtering.
- Persist and index action history in a database to enable audit trails and advanced querying.
- Enrich Swagger/OpenAPI metadata with XML comments, example payloads, and response codes for clearer client integration.
- Containerize the service using Docker; set up CI/CD pipelines (GitHub Actions, Azure DevOps) for automated testing and deployments.

---

*Prepared for the Infonetica assignment by Symbiot01 (Sahil Patel).*
