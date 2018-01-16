# AspNetCore.LinearAllocator

Linear Allocator service written in ASP.NET Core Web API

## Required

1. ASP.NET Core SDK
2. Microsoft Sql Server 


## Run the project

1. Create the target database
2. Update the connection string in `appsettings.Development.json`
3. Use the following `dotnet ef` commands to create the table
   ```
   ```
4. dotnet run  


## Create an Allocator

* Http Method: POST
* Defaul URL: http://localhost:5123/Allocator/Create
* Http Body sample:
  ```
  ```

## Get unique number from the Allocator

* Http Method: POST
* Defaul URL: http://localhost:5123/Allocator/GetNext/{key}


