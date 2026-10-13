# Forum App — ASP.NET Core Web API + Blazor

A small Reddit-style forum built step by step in the **DNP (.NET) course** at VIA University College.
Users can register, log in, write posts and comment on them — through a command-line client, a REST API and a Blazor web app.

## What it shows
- **Layered architecture**: entities, repository interfaces and interchangeable storage implementations
- **Repository pattern**: the same `IPostRepository`, `IUserRepository` and `ICommentRepository` contracts are implemented both **in memory** and in **JSON files**
- **REST Web API** with controllers for users, posts, comments and authentication, using **DTOs** to separate the API from the domain model
- **Blazor web client** that talks to the API through typed HTTP services, with a custom `AuthenticationStateProvider` for login
- **Swagger / OpenAPI** for trying the API in the browser
- A **CLI** client using the same repositories

## Architecture
```
Blazor client (BlazorApp1)  ──HTTP──▶  Web API (Controllers + DTOs)
                                            │
                                   Repository contracts
                                     │              │
                         InMemoryRepositories   FileRepository (JSON)
                                            │
                                         Entities
```
More detail, including every endpoint, is in [API_DOCUMENTATION.md](API_DOCUMENTATION.md).

## Project structure
| Folder | Purpose |
|---|---|
| `Server/Entities` | `User`, `Post`, `Comment` domain classes |
| `Server/RepositoryContracts` | Repository interfaces |
| `Server/InMemoryRepositories` | In-memory implementation |
| `Server/FileRepository` | JSON-file implementation used by the API |
| `Server/WebApi` | ASP.NET Core Web API (controllers, Swagger) |
| `Server/CLI` | Console client |
| `Shared/DTOs` | Request/response models shared by API and client |
| `BlazorApp1` | Blazor web app (login, users, posts, comments) |

## Run it
Requires the **.NET 8 SDK**.
```bash
cd Server/WebApi
dotnet run
```
Open the Swagger page shown in the console. Then, in a second terminal:
```bash
cd BlazorApp1
dotnet run
```

## Tech
C# · .NET 8 · ASP.NET Core Web API · Blazor · Swagger · JSON file storage

> The sample users in `users.json` are test data for the assignment. Passwords are stored in plain text on purpose to keep the exercise simple — not something to do in a real system.
