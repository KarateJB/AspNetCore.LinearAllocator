using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Allocator.DAL.Models;

namespace Allocator.DAL
{
    public class AllocatorDbContext : DbContext
    {
        public AllocatorDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<HiLo> HiLoMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Set NULLABLE Columns
            // modelBuilder.Entity<SvLog>().Property(x => x.CreateOn).IsRequired(false);
        }
    }
}