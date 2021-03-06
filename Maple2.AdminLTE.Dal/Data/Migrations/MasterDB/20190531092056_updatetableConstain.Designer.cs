﻿// <auto-generated />
using System;
using Maple2.AdminLTE.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Maple2.AdminLTE.Dal.Data.Migrations.MasterDB
{
    [DbContext(typeof(MasterDbContext))]
    [Migration("20190531092056_updatetableConstain")]
    partial class updatetableConstain
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressL1");

                    b.Property<string>("AddressL2");

                    b.Property<string>("AddressL3");

                    b.Property<string>("AddressL4");

                    b.Property<string>("CompanyCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("CompanyLogoPath");

                    b.Property<string>("CompanyName")
                        .IsRequired();

                    b.Property<string>("CompanyTaxId");

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<string>("Fax");

                    b.Property<bool>("Is_Active");

                    b.Property<string>("Telephone");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("CompanyCode")
                        .IsUnique();

                    b.ToTable("m_company");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<string>("DeptCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("DeptDesc");

                    b.Property<string>("DeptName")
                        .IsRequired();

                    b.Property<bool>("Is_Active");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("DeptCode", "CompanyCode")
                        .IsUnique();

                    b.ToTable("m_department");
                });
#pragma warning restore 612, 618
        }
    }
}
