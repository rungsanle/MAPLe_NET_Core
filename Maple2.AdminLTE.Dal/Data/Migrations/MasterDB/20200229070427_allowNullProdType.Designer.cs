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
    [Migration("20200229070427_allowNullProdType")]
    partial class allowNullProdType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_ArrivalType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArrivalTypeCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("ArrivalTypeDesc");

                    b.Property<string>("ArrivalTypeName")
                        .IsRequired();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("ArrivalTypeCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_ARRTYPEANDCOMP");

                    b.ToTable("m_arrivaltype");
                });

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

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressL1");

                    b.Property<string>("AddressL2");

                    b.Property<string>("AddressL3");

                    b.Property<string>("AddressL4");

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<int?>("CreditTerm");

                    b.Property<string>("CustomerCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("CustomerContact");

                    b.Property<string>("CustomerEmail");

                    b.Property<string>("CustomerName")
                        .IsRequired();

                    b.Property<string>("CustomerTaxId");

                    b.Property<string>("Fax");

                    b.Property<bool>("Is_Active");

                    b.Property<int?>("PriceLevel");

                    b.Property<string>("Remark");

                    b.Property<string>("Telephone");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("CustomerCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_CUSTANDCOMP");

                    b.ToTable("m_customer");
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
                        .IsUnique()
                        .HasName("IX_DEPTANDCOMP");

                    b.ToTable("m_department");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<string>("LocationCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("LocationDesc");

                    b.Property<string>("LocationName")
                        .IsRequired();

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.Property<int?>("WarehouseId");

                    b.HasKey("Id");

                    b.ToTable("m_location");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Machine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<string>("MachineCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("MachineName")
                        .IsRequired();

                    b.Property<int?>("MachineProdType");

                    b.Property<string>("MachineRemark");

                    b.Property<string>("MachineSize");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("MachineCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_MCANDCOMP");

                    b.ToTable("m_machine");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<int?>("LocationId");

                    b.Property<string>("MaterialCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("MaterialDesc1");

                    b.Property<string>("MaterialDesc2");

                    b.Property<string>("MaterialImagePath");

                    b.Property<string>("MaterialName")
                        .IsRequired();

                    b.Property<decimal?>("PackageStdQty");

                    b.Property<int?>("RawMatTypeId");

                    b.Property<int?>("UnitId");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.Property<int?>("WarehouseId");

                    b.HasKey("Id");

                    b.HasIndex("MaterialCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_MATANDCOMP");

                    b.ToTable("m_material");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_MaterialType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<string>("MatTypeCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("MatTypeDesc");

                    b.Property<string>("MatTypeName")
                        .IsRequired();

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("MatTypeCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_MATTYPECOMP");

                    b.ToTable("m_materialtype");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.Property<string>("action");

                    b.Property<string>("area");

                    b.Property<string>("controller");

                    b.Property<string>("imageClass");

                    b.Property<bool>("isParent");

                    b.Property<int>("menuseq");

                    b.Property<string>("nameOption");

                    b.Property<int?>("parentId");

                    b.Property<bool>("status");

                    b.HasKey("Id");

                    b.ToTable("m_menu");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Process", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<string>("ProcessCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("ProcessDesc");

                    b.Property<string>("ProcessName")
                        .IsRequired();

                    b.Property<int?>("ProcessSeq");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("ProcessCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_PROCESSCOMP");

                    b.ToTable("m_process");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<string>("GLCogsAccount")
                        .IsRequired();

                    b.Property<string>("GLInventAccount");

                    b.Property<string>("GLSalesAccount")
                        .IsRequired();

                    b.Property<bool>("Is_Active");

                    b.Property<int?>("LocationId");

                    b.Property<int?>("MachineId");

                    b.Property<int?>("MaterialTypeId");

                    b.Property<decimal?>("PackageStdQty");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("ProductDesc");

                    b.Property<string>("ProductImagePath");

                    b.Property<string>("ProductName")
                        .IsRequired();

                    b.Property<string>("ProductNameRef");

                    b.Property<int?>("ProductionTypeId");

                    b.Property<int?>("RevisionNo");

                    b.Property<decimal?>("SalesPrice1");

                    b.Property<decimal?>("SalesPrice2");

                    b.Property<decimal?>("SalesPrice3");

                    b.Property<decimal?>("SalesPrice4");

                    b.Property<decimal?>("SalesPrice5");

                    b.Property<int?>("UnitId");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.Property<int?>("WarehouseId");

                    b.HasKey("Id");

                    b.HasIndex("ProductCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_PRODUCTANDCOMP");

                    b.ToTable("m_product");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Product_Process", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<int>("ProcessId");

                    b.Property<int?>("ProcessSeq");

                    b.Property<int>("ProductId");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.ToTable("m_product_process");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_ProductionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<string>("ProdTypeCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("ProdTypeDesc");

                    b.Property<string>("ProdTypeName")
                        .IsRequired();

                    b.Property<int?>("ProdTypeSeq");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("ProdTypeCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_PRODTYPECOMP");

                    b.ToTable("m_productiontype");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_RawMaterialType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<string>("RawMatTypeCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("RawMatTypeDesc");

                    b.Property<string>("RawMatTypeName")
                        .IsRequired();

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("RawMatTypeCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_RAWMATTYPECOMP");

                    b.ToTable("m_rawmaterialtype");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<string>("UnitCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("UnitDesc");

                    b.Property<string>("UnitName")
                        .IsRequired();

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.HasKey("Id");

                    b.HasIndex("UnitCode")
                        .IsUnique();

                    b.ToTable("m_unit");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<int?>("DeptId");

                    b.Property<string>("EmpCode");

                    b.Property<bool>("Is_Active");

                    b.Property<string>("Position");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.Property<string>("UserCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("UserImagePath");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.Property<string>("aspnetuser_Id")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("UserCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_USERANDCOMP");

                    b.ToTable("m_user");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Vendor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressL1");

                    b.Property<string>("AddressL2");

                    b.Property<string>("AddressL3");

                    b.Property<string>("AddressL4");

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<int?>("CreditTerm");

                    b.Property<string>("Fax");

                    b.Property<bool>("Is_Active");

                    b.Property<int?>("PriceLevel");

                    b.Property<string>("Remark");

                    b.Property<string>("Telephone");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.Property<string>("VendorCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("VendorContact");

                    b.Property<string>("VendorEmail");

                    b.Property<string>("VendorName")
                        .IsRequired();

                    b.Property<string>("VendorTaxId");

                    b.HasKey("Id");

                    b.HasIndex("VendorCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_CUSTANDCOMP");

                    b.ToTable("m_vendor");
                });

            modelBuilder.Entity("Maple2.AdminLTE.Bel.M_Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(30);

                    b.Property<int?>("Created_By");

                    b.Property<DateTime?>("Created_Date");

                    b.Property<bool>("Is_Active");

                    b.Property<int?>("Updated_By");

                    b.Property<DateTime?>("Updated_Date");

                    b.Property<string>("WarehouseCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("WarehouseDesc");

                    b.Property<string>("WarehouseName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("WarehouseCode", "CompanyCode")
                        .IsUnique()
                        .HasName("IX_WHANDCOMP");

                    b.ToTable("m_warehouse");
                });
#pragma warning restore 612, 618
        }
    }
}
