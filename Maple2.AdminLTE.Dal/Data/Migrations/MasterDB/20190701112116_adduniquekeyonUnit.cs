using Microsoft.EntityFrameworkCore.Migrations;

namespace Maple2.AdminLTE.Dal.Data.Migrations.MasterDB
{
    public partial class adduniquekeyonUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_m_unit_UnitCode",
                table: "m_unit",
                column: "UnitCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_m_unit_UnitCode",
                table: "m_unit");
        }
    }
}
