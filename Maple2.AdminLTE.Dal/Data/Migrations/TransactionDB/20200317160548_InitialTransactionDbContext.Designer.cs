﻿// <auto-generated />
using System;
using Maple2.AdminLTE.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Maple2.AdminLTE.Dal.Data.Migrations.TransactionDB
{
    [DbContext(typeof(TransactionDbContext))]
    [Migration("20200317160548_InitialTransactionDbContext")]
    partial class InitialTransactionDbContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Maple2.AdminLTE.Bel.T_Arrival_Detail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ArrivalId");

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<string>("DetailRemark");

                    b.Property<string>("GenLabelStatus");

                    b.Property<bool>("Is_Active");

                    b.Property<int?>("LineNo");

                    b.Property<DateTime?>("LotDate");

                    b.Property<string>("LotNo");

                    b.Property<string>("MaterialCode");

                    b.Property<string>("MaterialDesc");

                    b.Property<int?>("MaterialId");

                    b.Property<string>("MaterialName");

                    b.Property<int?>("NoOfLabel");

                    b.Property<decimal?>("OrderQty");

                    b.Property<int?>("PoLineNo");

                    b.Property<decimal?>("RecvQty");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.ToTable("t_arrival_dtl");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.T_Arrival_Detail_Sub", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ArrivalId");

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<int?>("DtlLineNo");

                    b.Property<bool>("Is_Active");

                    b.Property<decimal?>("LabelQty");

                    b.Property<int?>("MaterialId");

                    b.Property<int?>("NoOfLabel");

                    b.Property<string>("SubDetail");

                    b.Property<int?>("SubLineNo");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.ToTable("t_arrival_dtl_sub");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.T_Arrival_Header", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("ArrivalDate")
                        .IsRequired();

                    b.Property<string>("ArrivalNo")
                        .HasMaxLength(30);

                    b.Property<string>("ArrivalRemark");

                    b.Property<int?>("ArrivalTypeId");

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<DateTime?>("DocRefDate");

                    b.Property<string>("DocRefNo");

                    b.Property<bool>("Is_Active");

                    b.Property<string>("PurchaseOrderNo")
                        .HasMaxLength(30);

                    b.Property<int?>("RawMatTypeId");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.Property<int?>("VendorId");

                    b.HasKey("Id");

                    b.HasIndex("ArrivalNo", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_ARRIVALANDCOMP");

                    b.ToTable("t_arrival_hdr");
                });
#pragma warning restore 612, 618
        }
    }
}
