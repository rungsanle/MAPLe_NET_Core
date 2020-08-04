using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maple2.AdminLTE.Dal.Data.Migrations.MasterDB
{
    public partial class addAllMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_arrivaltype",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    ArrivalTypeCode = table.Column<string>(maxLength: 30, nullable: false),
                    ArrivalTypeName = table.Column<string>(nullable: false),
                    ArrivalTypeDesc = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_arrivaltype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    CustomerCode = table.Column<string>(maxLength: 30, nullable: false),
                    CustomerName = table.Column<string>(nullable: false),
                    AddressL1 = table.Column<string>(nullable: true),
                    AddressL2 = table.Column<string>(nullable: true),
                    AddressL3 = table.Column<string>(nullable: true),
                    AddressL4 = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    CustomerEmail = table.Column<string>(nullable: true),
                    CustomerContact = table.Column<string>(nullable: true),
                    CreditTerm = table.Column<int>(nullable: true),
                    PriceLevel = table.Column<int>(nullable: true),
                    CustomerTaxId = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    LocationCode = table.Column<string>(maxLength: 30, nullable: false),
                    LocationName = table.Column<string>(nullable: false),
                    LocationDesc = table.Column<string>(nullable: true),
                    WarehouseId = table.Column<int>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_machine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    MachineCode = table.Column<string>(maxLength: 30, nullable: false),
                    MachineName = table.Column<string>(nullable: false),
                    MachineProdType = table.Column<int>(nullable: false),
                    MachineSize = table.Column<string>(nullable: true),
                    MachineRemark = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_machine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_material",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    MaterialCode = table.Column<string>(maxLength: 30, nullable: false),
                    MaterialName = table.Column<string>(nullable: false),
                    MaterialDesc1 = table.Column<string>(nullable: true),
                    MaterialDesc2 = table.Column<string>(nullable: true),
                    RawMatTypeId = table.Column<int>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    PackageStdQty = table.Column<decimal>(nullable: true),
                    WarehouseId = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true),
                    MaterialImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_materialtype",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    MatTypeCode = table.Column<string>(maxLength: 30, nullable: false),
                    MatTypeName = table.Column<string>(nullable: false),
                    MatTypeDesc = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_materialtype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_process",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    ProcessCode = table.Column<string>(maxLength: 30, nullable: false),
                    ProcessName = table.Column<string>(nullable: false),
                    ProcessDesc = table.Column<string>(nullable: true),
                    ProcessSeq = table.Column<int>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_process", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 30, nullable: false),
                    ProductName = table.Column<string>(nullable: false),
                    ProductNameRef = table.Column<string>(nullable: true),
                    ProductDesc = table.Column<string>(nullable: true),
                    MaterialTypeId = table.Column<int>(nullable: true),
                    ProductionTypeId = table.Column<int>(nullable: true),
                    MachineId = table.Column<int>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    PackageStdQty = table.Column<decimal>(nullable: true),
                    SalesPrice1 = table.Column<decimal>(nullable: true),
                    SalesPrice2 = table.Column<decimal>(nullable: true),
                    SalesPrice3 = table.Column<decimal>(nullable: true),
                    SalesPrice4 = table.Column<decimal>(nullable: true),
                    SalesPrice5 = table.Column<decimal>(nullable: true),
                    GLSalesAccount = table.Column<string>(nullable: false),
                    GLInventAccount = table.Column<string>(nullable: true),
                    GLCogsAccount = table.Column<string>(nullable: false),
                    RevisionNo = table.Column<int>(nullable: true),
                    WarehouseId = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true),
                    ProductImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_product_process",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ProcessId = table.Column<int>(nullable: false),
                    ProcessSeq = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_product_process", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_productiontype",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    ProdTypeCode = table.Column<string>(maxLength: 30, nullable: false),
                    ProdTypeName = table.Column<string>(nullable: false),
                    ProdTypeDesc = table.Column<string>(nullable: true),
                    ProdTypeSeq = table.Column<int>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_productiontype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_rawmaterialtype",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    RawMatTypeCode = table.Column<string>(maxLength: 30, nullable: false),
                    RawMatTypeName = table.Column<string>(nullable: false),
                    RawMatTypeDesc = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_rawmaterialtype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_unit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    UnitCode = table.Column<string>(maxLength: 30, nullable: false),
                    UnitName = table.Column<string>(nullable: false),
                    UnitDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_vendor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    VendorCode = table.Column<string>(maxLength: 30, nullable: false),
                    VendorName = table.Column<string>(nullable: false),
                    AddressL1 = table.Column<string>(nullable: true),
                    AddressL2 = table.Column<string>(nullable: true),
                    AddressL3 = table.Column<string>(nullable: true),
                    AddressL4 = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    VendorEmail = table.Column<string>(nullable: true),
                    VendorContact = table.Column<string>(nullable: true),
                    CreditTerm = table.Column<int>(nullable: true),
                    PriceLevel = table.Column<int>(nullable: true),
                    VendorTaxId = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_vendor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_warehouse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    WarehouseCode = table.Column<string>(maxLength: 30, nullable: false),
                    WarehouseName = table.Column<string>(nullable: false),
                    WarehouseDesc = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_warehouse", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ARRTYPEANDCOMP",
                table: "m_arrivaltype",
                columns: new[] { "ArrivalTypeCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CUSTANDCOMP",
                table: "m_customer",
                columns: new[] { "CustomerCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MCANDCOMP",
                table: "m_machine",
                columns: new[] { "MachineCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MATANDCOMP",
                table: "m_material",
                columns: new[] { "MaterialCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MATTYPECOMP",
                table: "m_materialtype",
                columns: new[] { "MatTypeCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PROCESSCOMP",
                table: "m_process",
                columns: new[] { "ProcessCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTANDCOMP",
                table: "m_product",
                columns: new[] { "ProductCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PRODTYPECOMP",
                table: "m_productiontype",
                columns: new[] { "ProdTypeCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RAWMATTYPECOMP",
                table: "m_rawmaterialtype",
                columns: new[] { "RawMatTypeCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CUSTANDCOMP",
                table: "m_vendor",
                columns: new[] { "VendorCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WHANDCOMP",
                table: "m_warehouse",
                columns: new[] { "WarehouseCode", "CompanyCode" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_arrivaltype");

            migrationBuilder.DropTable(
                name: "m_customer");

            migrationBuilder.DropTable(
                name: "m_location");

            migrationBuilder.DropTable(
                name: "m_machine");

            migrationBuilder.DropTable(
                name: "m_material");

            migrationBuilder.DropTable(
                name: "m_materialtype");

            migrationBuilder.DropTable(
                name: "m_process");

            migrationBuilder.DropTable(
                name: "m_product");

            migrationBuilder.DropTable(
                name: "m_product_process");

            migrationBuilder.DropTable(
                name: "m_productiontype");

            migrationBuilder.DropTable(
                name: "m_rawmaterialtype");

            migrationBuilder.DropTable(
                name: "m_unit");

            migrationBuilder.DropTable(
                name: "m_vendor");

            migrationBuilder.DropTable(
                name: "m_warehouse");
        }
    }
}
