using Microsoft.EntityFrameworkCore.Migrations;

namespace Maple2.AdminLTE.Dal.Data.Migrations.MasterDB
{
    public partial class updatetableConstain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_m_company_CompanyCode",
                table: "m_company",
                column: "CompanyCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_m_company_CompanyCode",
                table: "m_company");
        }
    }
}
