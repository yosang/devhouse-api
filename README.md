[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/tPy1EiLa)

![](http://images.restapi.co.za/pvt/Noroff-64.png)
# Noroff
## Back-end Development Year 2
### BET - Course Assignment 

Classroom repository for Noroff back-end development 2 - BET Course Assignment.

Instruction for the course assignment is in the LMS (Moodle) system of Noroff.
[https://lms.noroff.no](https://lms.noroff.no)

![](http://images.restapi.co.za/pvt/important_icon.png)

You will not be able to make any submission after the deadline of the course assignment. Make sure to make all your commit **BEFORE** the deadline

![](http://images.restapi.co.za/pvt/help_small.png)

If you are unsure of any instructions for the course assignment, contact out to your teacher on **Microsoft Teams**.

**REMEMBER** Your Moodle LMS submission must have your repository link **AND** your Github username in the text file.

---

# Application setup instructions

## appsettings.json
This application relies on `appsettings.json` for configuration, this file wont be present when you clone this repo, create it with the following command: `touch appsettings.json` or manually, and follow the configuration instructions below.

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

## HTTPS configuration
This API is configured to use the https by default, since we are working with JWT over the network.
With `UseHttpsRedirection()`, any http connection attempt will be redirected to https, this means that 
our communication protocol `(request, response)` is always encrypted, and JWT tokens are protected.

When in development, we need a ASP.NET self-signed certificate, here are some command we can manage certificates with:

- `dotnet dev-certs https --check` - Lists current valid certificate, here we can see the validation and expiration date.
- `dotnet dev-certs https --trust` - Requests a ASP.NET certificate if missing and marks it as trusted locally.
- `dotnet dev-certs https --clean` - Removes any existing certificates.

The certificate is fine for development, but wont work for production, in which case we need a real certificate provider, 
the command above creates a fake certificate that tells our local clients (browsers) to trust the certificate provided.

To bypass the HTTPS profile altogether, run the http profile using `dotnet run --launch-profile http`

# Instructions to run the application

1. Make sure to clone the repo first with `git clone`.
2. Move into the project directory with `cd <project>`.
3. Make sure to install the required packages with `dotnet restore`.
4. The `Migrations` folder contains everything needed for the database including:
    - Tables and relationships configuration.
    - Seed data
5. Before using the migrations, make sure you have the `Entity Frame Work Core` tool installed, if nof, install with `dotnet tool install --global dotnet-ef` .
6. Synchronize and update the database with `dotnet ef database update`.
7. Run the application with `dotnet run`.
8. To test the available endpoints visit `https://localhost:7264/swagger`.

# Instructions to create needed Migrations
There is no need to create Migrations manually, its already provided, run `dotnet ef database update` to sync the database.

# Instructions to test authentication & authorization
I chose to implement a more realistic authentication and authorization design, beyond the minimal assignment requirement of JWT protection on CRUD endpoints.

## Authentication
Handled by a single `Login` on `Auth` endpoint, which validates user credentials. On successful login, a JWT is generated containing claims such as `roleId`, `roleName`, `developerId` and `teamId`. These claims are extracted using relevant methods using the `IHttpContextAccessor` type, which is injected on [TokenService.cs](./Services/Auth/TokenService.cs). This allows us to authenticate the a client through their token, instead of having to make additional database queriy checks.

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

### Admin
- email: admin@dev.com
- password: admin1234

### TeamLead
- email: lead@dev.com
- password: lead1234

### Developer
- email: dev@dev.com
- password: developer1234

# Connection String structure for MySQL Database connection
The following entry must be present in `appsettings.json`, replace the placeholders with your actual database details:

```json
 "ConnectionStrings": {
        "Default": "server=localhost;database=devhouse;user=<username>;password=<password>"
    },
```

# Additional external libraries/packages used
This API is built with `.NET 9.0`.

|Top-level Package |Requested |Resolved|
|---|---|---|
|Microsoft.AspNetCore.Authentication.JwtBearer|      9.0.*    |   9.0.14  |
|Microsoft.AspNetCore.OpenApi                 |      9.0.13   |   9.0.13  |
|Microsoft.EntityFrameworkCore.Design         |      9.0.*    |   9.0.14  |
|MySql.EntityFrameworkCore                    |      9.0.*    |   9.0.11  |
|Swashbuckle.AspNetCore                       |      9.0.*    |   9.0.6   |
|System.IdentityModel.Tokens.Jwt |  8.16.0    |  8.16.0 |