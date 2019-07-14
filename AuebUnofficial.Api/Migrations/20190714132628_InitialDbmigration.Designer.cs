﻿// <auto-generated />
using System;
using AuebUnofficial.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuebUnofficial.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190714132628_InitialDbmigration")]
    partial class InitialDbmigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AuebUnofficial.Api.Model.Course", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Token")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("AuebUnofficial.Api.Model.RSSAnouncements", b =>
                {
                    b.Property<string>("Kind");

                    b.Property<DateTime>("LastUpdatedDateTime")
                        .HasColumnName("LastUpdatedDateTime");

                    b.Property<string>("Link")
                        .IsRequired();

                    b.HasKey("Kind");

                    b.ToTable("RSSAnouncements");
                });
#pragma warning restore 612, 618
        }
    }
}
