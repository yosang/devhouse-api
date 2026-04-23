# Devhouse API
A backend API for managing projects, teams, developers and roles. Built with ASP.NET Core and Entity Frame Work Core in .NET 9 with C#.

## Features
- CRUD operations for Projects, Developers, ProjectTypes, Roles and Teams.
- JWT Authentication.
- Database relationships with EF Core and migrations with dotnet-ef tool.
- Swagger UI for interactive API documentation with example requests and support for authentication.
- Dependency Injection for DbContext and other services.

## Technologies
- **Language**: C# / .NET 9
- **ORM**: Entity Framework Core
- **Auth**: JWT with Role-Based Access Control (RBAC)
- **Documentation**: Swagger / OpenAPI

## Project Structure

```
├── Config
│   ├── CorsConfig.cs
│   ├── DatabaseConfig.cs
│   ├── JwtConfig.cs
│   ├── JwtSettings.cs
│   ├── ServicesConfig.cs
│   └── SwaggerConfig.cs
├── Controllers
│   ├── AuthController.cs
│   ├── DevelopersController.cs
│   ├── ProjectsController.cs
│   ├── ProjectTypesController.cs
│   ├── RolesController.cs
│   └── TeamsController.cs
├── Data
│   ├── DatabaseContext.cs
│   └── Models
│       ├── Developer.cs
│       ├── Project.cs
│       ├── ProjectType.cs
│       ├── Role.cs
│       └── Team.cs
├── DTOs
├── Migrations
└── Services
    ├── Auth
    │   ├── AuthService.cs
    │   └── TokenService.cs
    ├── DeveloperService.cs
    ├── Factory
    │   ├── ProblemResultService.cs
    │   └── ServiceResultService.cs
    ├── ProjectService.cs
    ├── ProjectTypeService.cs
    ├── RoleService.cs
    └── TeamService.cs
```


## Grading Feedback & Highlights
This API was built as an academic project, being my first proper backend project built entirely in .NET with C#, im proud that it received such good feedback:
> "Project was set up with the correct technologies. Excellent use of dependency injection. Swagger documentation complete with required descriptions." — Grading Feedback

**Challenges and Implementations**:

There were a few areas that challenged me throughout this project and that I considered as key achievments:

- **RBAC**: Implementation of granular permissions (Admins, TeamLEad and Developers) via custom claims.
- **Architecture**: Proper and clean use of Separation of Concerns and Dependency Injection for a maintainable codebase that can easily be scaled at any point in the future.
- **Security**: Secured communication through HTTPS and JWT-protected routes.

**Areas for improvement**

There were a few things I realized I wanted to dive deeper on and maybe add to the roadmap:

- [ ] Add input validation to prevent duplicate records.
- [ ] Add an additional Users model for decoupled authentication, currently authentication is implemented through the Developers model.
- [ ] Add logging with Serilog.

**What I learned**
- Implementing a tidy architecture with proper use of separation of concerns and dependency injection.
- How to implement and design API using controllers in ASP.NET core
- How to implement and integrate Entity Framework Core code-first relationships and migrations with an API in ASP.NET Core
- How to implement and secure the API with JWT in ASP.NET core
- How to implement and document the API with Swagger in ASP.NET core


# Application setup instructions

## appsettings.json
This application relies on `appsettings.json` for configuration, this file wont be present when you clone this repo, create it with the following command: `touch appsettings.json` or manually, and follow the configuration instructions below.

Example

```json
{
  "ConnectionStrings": {
    // ...
  },
  "JwtSettings":
  {
    // ...
  }
}

```

## JWT Configuration
The following entry must be present in `appsettings.json`, replace the placeholders with your actual JWT details:

```json
    "JwtSettings":
    {
        "SecretKey":"<ALongStringThatIsSuperSecretOfAtLeast60Characters>",
        "Issuer": "devhouse",
        "Audience": "noroff",
        "ExpiryMinutes": 60
    }
```

## Connection String structure for MySQL Database connection
The following entry must be present in `appsettings.json`, replace the placeholders with your actual database details:

```json
 "ConnectionStrings": {
        "Default": "server=localhost;database=devhouse;user=<username>;password=<password>"
    },
```

# Instructions to run the application

1. Make sure to clone the repo first with `git clone`.
2. Move into the project directory with `cd <project>`.
3. Create `appsettings.json`, configure `ConnectionStrings` and `JwtSettings`, see the samples above.
4. Make sure to install the required packages with `dotnet restore`.
5. The `Migrations` folder contains everything needed for the database including:
    - Tables and relationships configuration.
    - Seed data
6. Before using the migrations, make sure you have the `Entity Frame Work Core` tool installed, if not, install with `dotnet tool install --global dotnet-ef` .
7. Synchronize and update the database with `dotnet ef database update`.
8. Run the application with one of those commands:
    - `dotnet run` (requires https certificate, read about this in the section on `https`).
    - `dotnet run --launch-profile http`
9. To test the available endpoints visit `/swagger`.

# Instructions to create needed Migrations
There is no need to create Migrations manually, its already provided, run `dotnet ef database update` to sync the database.

# Instructions to test authentication & authorization
I chose to implement a more realistic authentication and authorization design, beyond the minimal assignment requirement of JWT protection on CRUD endpoints.

## Authentication
Handled by a single `Login` on `Auth` endpoint, which validates user credentials. On successful login, a JWT is generated containing claims such as `roleId`, `roleName`, `developerId` and `teamId`. These claims are extracted using relevant methods using the `IHttpContextAccessor` type, which is injected on [TokenService.cs](./Services/Auth/TokenService.cs). This allows us to immplement authorization checks through the token, instead of having to make additional database query checks.

## Authorization
For authorization, I chose to implement a Role Based Access Control (RBAC) system. The roles are `Developer`, `TeamLead` and `Admin`.
- `Admin` - Have full access to all resources.
- `Teamlead` - Can only modify within their team, limited to developers only. They cannot modify other `TeamLeads` or `Admins`.
- `Developer` - Can only modify their own data.

All logic is centralized in the [AuthService](./Services/Auth/AuthService.cs) layer through specific methods.

While we could have implemented controller attributes, this approach provides more granular detailed RBAC.

Currently all `GET` endpoints are publicly accessible, while `POST`, `PUT` and `DELETE` endpoints are restricted. This protects integrity, while leaving some data open. I also implemented DTO's for the `GET` endpoints to hide sensitive information, such as `passwords` and `emails`.   

## Testing
To make the system easier to test, I provided seed data with developers of different roles across multiple teams.

> "Note: These passwords are for the development seed data only. In the database, these are stored as hashed strings."

### Admin
- email: admin@dev.com
- password: admin1234

### TeamLead
- email: lead@dev.com
- password: lead1234

### Developer
- email: dev@dev.com
- password: developer1234

# HTTPS configuration
This API is configured to use the https by default, since we are working with JWT over the network.
With `UseHttpsRedirection()`, any http connection attempt will be redirected to https, this means that 
our communication protocol `(request, response)` is always encrypted, and JWT tokens are protected.

When in development, we need a ASP.NET self-signed certificate, here are some commands we can manage certificates with:

- `dotnet dev-certs https --check` - Lists current valid certificate, here we can see the validation and expiration date.
- `dotnet dev-certs https --trust` - Requests a ASP.NET certificate if missing and marks it as trusted locally.
- `dotnet dev-certs https --clean` - Removes any existing certificates.

The certificate is fine for development, but wont work for production, in which case we need a real certificate provider, 
the command above creates a fake certificate that tells our local clients (browsers) to trust the certificate provided.

To bypass the HTTPS profile altogether, run the http profile using `dotnet run --launch-profile http`

# Additional external libraries/packages used
This API is built with `.NET 9.0`.

|Top-level Package |Requested |
|---|---|
|Microsoft.AspNetCore.Authentication.JwtBearer|      9.0.*    |
|Microsoft.AspNetCore.OpenApi                 |      9.0.13   |
|Microsoft.EntityFrameworkCore.Design         |      9.0.*    |
|MySql.EntityFrameworkCore                    |      9.0.*    |
|Swashbuckle.AspNetCore                       |      9.0.*    |
|System.IdentityModel.Tokens.Jwt |  8.16.0    |
