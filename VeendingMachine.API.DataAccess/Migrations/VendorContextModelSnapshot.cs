﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VeendingMachine.API.DataAccess.DatabaseContext;

#nullable disable

namespace VeendingMachine.API.DataAccess.Migrations
{
    [DbContext(typeof(VendorContext))]
    partial class VendorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("VeendingMachine.API.DataAccess.Entities.DepositStack", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("Denomination")
                        .HasColumnType("int");

                    b.Property<Guid>("MoneyUnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MoneyUnitId");

                    b.ToTable("DepositStacks");
                });

            modelBuilder.Entity("VeendingMachine.API.DataAccess.Entities.MoneyUnit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MoneyUnits");
                });

            modelBuilder.Entity("VeendingMachine.API.DataAccess.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("VeendingMachine.API.DataAccess.Entities.Purchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Paid")
                        .HasColumnType("bit");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("VeendingMachine.API.DataAccess.Entities.DepositStack", b =>
                {
                    b.HasOne("VeendingMachine.API.DataAccess.Entities.MoneyUnit", "MoneyUnit")
                        .WithMany("DepositStack")
                        .HasForeignKey("MoneyUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MoneyUnit");
                });

            modelBuilder.Entity("VeendingMachine.API.DataAccess.Entities.Purchase", b =>
                {
                    b.HasOne("VeendingMachine.API.DataAccess.Entities.Product", "Product")
                        .WithMany("Purchase")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("VeendingMachine.API.DataAccess.Entities.MoneyUnit", b =>
                {
                    b.Navigation("DepositStack");
                });

            modelBuilder.Entity("VeendingMachine.API.DataAccess.Entities.Product", b =>
                {
                    b.Navigation("Purchase");
                });
#pragma warning restore 612, 618
        }
    }
}
