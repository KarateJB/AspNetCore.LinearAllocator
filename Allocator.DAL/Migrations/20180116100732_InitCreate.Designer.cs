﻿// <auto-generated />
using Allocator.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Allocator.DAL.Migrations
{
    [DbContext(typeof(AllocatorDbContext))]
    [Migration("20180116100732_InitCreate")]
    partial class InitCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Allocator.DAL.Models.HiLo", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200);

                    b.Property<long>("MaxValue")
                        .HasColumnType("bigint");

                    b.Property<long>("NextHi")
                        .HasColumnType("bigint");

                    b.HasKey("Key");

                    b.ToTable("HiLos");
                });
#pragma warning restore 612, 618
        }
    }
}
