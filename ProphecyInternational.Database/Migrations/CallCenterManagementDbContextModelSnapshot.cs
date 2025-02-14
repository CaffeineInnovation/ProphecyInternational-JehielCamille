﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProphecyInternational.Database.DbContexts;

#nullable disable

namespace ProphecyInternational.Database.Migrations
{
    [DbContext(typeof(CallCenterManagementDbContext))]
    partial class CallCenterManagementDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProphecyInternational.Common.Models.AgentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneExtension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Agents");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "zgeronimo@testdomain.com",
                            Name = "Zeylee Geronimo",
                            PhoneExtension = "1001",
                            Status = 0
                        },
                        new
                        {
                            Id = 2,
                            Email = "xnerier@testdomain.com",
                            Name = "Xanlaneron Nerier",
                            PhoneExtension = "1002",
                            Status = 1
                        },
                        new
                        {
                            Id = 3,
                            Email = "jballa@testdomain.com",
                            Name = "Jehiel Balla",
                            PhoneExtension = "1003",
                            Status = 2
                        });
                });

            modelBuilder.Entity("ProphecyInternational.Common.Models.CallModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AgentId")
                        .HasColumnType("int");

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Calls");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AgentId = 1,
                            CustomerId = "CUST001",
                            Notes = "Customer called regarding login issue",
                            StartTime = new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = 1
                        },
                        new
                        {
                            Id = 2,
                            AgentId = 2,
                            CustomerId = "CUST002",
                            EndTime = new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Notes = "Resolved billing discrepancy",
                            StartTime = new DateTime(2025, 2, 10, 23, 30, 0, 0, DateTimeKind.Unspecified),
                            Status = 2
                        });
                });

            modelBuilder.Entity("ProphecyInternational.Common.Models.CustomerModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastContactDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = "CUST001",
                            Email = "mclara@test.com",
                            Name = "Maria Clara",
                            PhoneNumber = "1234567890"
                        },
                        new
                        {
                            Id = "CUST002",
                            Email = "cibarra@test.com",
                            LastContactDate = new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Crisostomo Ibarra",
                            PhoneNumber = "1234567891"
                        });
                });

            modelBuilder.Entity("ProphecyInternational.Common.Models.TicketModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AgentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("Resolution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Tickets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AgentId = 1,
                            CreatedAt = new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CustomerId = "CUST001",
                            Description = "Issue with login",
                            Priority = 2,
                            Status = 0,
                            UpdatedAt = new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            AgentId = 2,
                            CreatedAt = new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CustomerId = "CUST002",
                            Description = "Billing discrepancy",
                            Priority = 1,
                            Status = 1,
                            UpdatedAt = new DateTime(2025, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("ProphecyInternational.Common.Models.CallModel", b =>
                {
                    b.HasOne("ProphecyInternational.Common.Models.AgentModel", null)
                        .WithMany()
                        .HasForeignKey("AgentId");

                    b.HasOne("ProphecyInternational.Common.Models.CustomerModel", null)
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("ProphecyInternational.Common.Models.TicketModel", b =>
                {
                    b.HasOne("ProphecyInternational.Common.Models.AgentModel", null)
                        .WithMany()
                        .HasForeignKey("AgentId");

                    b.HasOne("ProphecyInternational.Common.Models.CustomerModel", null)
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });
#pragma warning restore 612, 618
        }
    }
}
