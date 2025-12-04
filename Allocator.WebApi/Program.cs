using Allocator.DAL;
using Allocator.Service;
using NLog.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddControllers();

#region CORS
// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
#endregion

#region Singleton HiLo-GetValue Provider
var dbFactory = new DbContextFactory(builder.Environment.EnvironmentName);
builder.Services.AddSingleton<IAllocatorGetValProvider>(provider => new AllocatorGetValProvider(dbFactory));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    (new DbContextSeedData()).Seed();
}

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
