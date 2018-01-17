# AspNetCore.LinearAllocator

Linear Allocator service written in ASP.NET Core Web API

## Required

1. [.NET Core SDK](https://www.microsoft.com/net/download/windows)
2. [Microsoft Sql Server](https://www.microsoft.com/zh-tw/sql-server/sql-server-editions-express)


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


