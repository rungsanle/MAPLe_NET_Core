using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maple2.AdminLTE.Dal.Data.Migrations.MasterDB
{
    public partial class InitialMasterDataContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_company",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: false),
                    CompanyName = table.Column<string>(nullable: false),
                    CompanyLogoPath = table.Column<string>(nullable: true),
                    AddressL1 = table.Column<string>(nullable: true),
                    AddressL2 = table.Column<string>(nullable: true),
                    AddressL3 = table.Column<string>(nullable: true),
                    AddressL4 = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    CompanyTaxId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "m_department",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    DeptCode = table.Column<string>(maxLength: 30, nullable: false),
                    DeptName = table.Column<string>(nullable: false),
                    DeptDesc = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_department", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_m_department_DeptCode_CompanyCode",
                table: "m_department",
                columns: new[] { "DeptCode", "CompanyCode" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_company");

            migrationBuilder.DropTable(
                name: "m_department");
        }
    }
}
