﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Weather.Temperature.DataAccess;

#nullable disable

namespace Weather.Temperature.Migrations
{
    [DbContext(typeof(TemperatureDbContext))]
    [Migration("20240327182315_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Weather.Temperature.DataAccess.Temperature", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Ddd")
                        .HasColumnType("text");

                    b.Property<decimal>("TempHighC")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TempLowC")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("temperature", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
