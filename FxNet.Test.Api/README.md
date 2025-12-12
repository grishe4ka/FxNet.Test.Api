# FxNet Test Backend (ASP.NET Core 8)

## Description
This project is a REST API on ASP.NET Core 8 using PostgreSQL (code-first approach, EF Core).

## Stack
- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- JWT (JSON Web Tokens)
- xUnit (unit tests only prepared)
- DB Seeder (only prepared)

## Launch
### Prepare PostgreSQL
### Configure connection string in appsettings.json (database "fxnet_test")
"ConnectionStrings": {
"Default": "Host=localhost;Port=5432;Database=fxnet_test;Username=postgres;Password=postgres"
}
### Apply migrations in the FxNet.Test.Api folder:
dotnet ef migrations add Init
dotnet ef database update
### Run the API
dotnet run
or
from Visual Studio by clicking Run on Https

Swagger UI (HTTP): 
http://localhost:5666/swagger

Swagger UI (HTTPS): 
https://localhost:7666/swagger

###Authentication
http
POST /api.user.partner.rememberMe?code=some-code
The response will be:
json
{
"token": "<JWT>"
}
Use this token in other applications Requests and enter it on Swagger page

http
Authorization: Bearer <JWT>
Primary Endpoints
Journal:
POST /api.user.journal.getRange
POST /api.user.journal.getSingle

Partner (authentication):
POST /api.user.partner.rememberMe

Tree:
POST /api.user.tree.get

Tree Nodes:
POST /api.user.tree.node.create
POST /api.user.tree.node.delete
POST /api.user.tree.node.rename
