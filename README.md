# AspNetCore.LinearAllocator

Linear Allocator service written in ASP.NET Core Web API

## Required

1. [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. [Microsoft Sql Server](https://www.microsoft.com/sql-server/sql-server-downloads)


## Run the project

1. Create the target database
2. Update the connection string in `appsettings.Development.json`
3. Use the following `dotnet ef` commands to create the table
   ```
   cd Allocator.WebApi
   dotnet ef  --project ../Allocator.DAL --startup-project . migrations add InitCreate --context AllocatorDbContext
   dotnet ef  --project ../Allocator.DAL --startup-project . database update
   ```
4. dotnet run  


## Create an Allocator

* Http Method: POST
* Defaul URL: http://localhost:5123/api/Allocator/Create
* Http Body: (JSON sample)
  ```
  {
	"Key":"TMS",
	"NextHi":1,
	"MaxValue":10
  }
  ```

## Get unique number from the Allocator

* Http Method: POST
* Defaul URL: http://localhost:5123/api/Allocator/GetNext/{key}

## Changelog

### Version 2.0 - .NET 8 Upgrade
- Upgraded all projects to target .NET 8
- Updated all NuGet packages to latest .NET 8 compatible versions
- Migrated from legacy `WebHost` to modern `WebApplication` builder pattern
- Replaced deprecated `Microsoft.AspNetCore.All` metapackage with specific package references
- Updated Entity Framework Core from 2.0.1 to 8.0.11
- Replaced deprecated `System.Data.SqlClient` with `Microsoft.Data.SqlClient`
- Updated deprecated EF Core APIs:
  - `Relational()` → `GetTableName()`
  - `ExecuteSqlCommand()` → `ExecuteSqlRaw()`
- Fixed deprecated `IHostingEnvironment` to `IWebHostEnvironment`
- Updated NLog packages to latest stable versions
- Removed obsolete `DotNetCliToolReference` elements from project files


