using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Allocator.DAL
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AllocatorDbContext>, IDisposable
    {
        private AllocatorDbContext _dbContext = null;
        private string _envName = string.Empty;

        public DbContextFactory()
        {
        }

        public DbContextFactory(string envName = "")
        {
            this._envName = envName;
        }

        public AllocatorDbContext CreateDbContext(string[] args = null)
        {
            string settingPath = string.Empty;
            if (!string.IsNullOrEmpty(this._envName))
                settingPath = $"appsettings.{this._envName}.json";
            else
                settingPath = $"appsettings.json";


            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingPath)
                .Build();

            var builder = new DbContextOptionsBuilder<AllocatorDbContext>();

            var connectionString = configuration["Data:DefaultConnection:ConnectionString"];


            builder.UseSqlServer(connectionString);

            this._dbContext = new AllocatorDbContext(builder.Options);
            return this._dbContext;
        }

        public void Dispose()
        {
            if (this._dbContext != null)
                this._dbContext.Dispose();

        }
    }
}