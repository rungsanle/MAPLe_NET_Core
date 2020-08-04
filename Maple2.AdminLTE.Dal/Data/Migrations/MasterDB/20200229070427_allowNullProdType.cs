using Microsoft.EntityFrameworkCore.Migrations;

namespace Maple2.AdminLTE.Dal.Data.Migrations.MasterDB
{
    public partial class allowNullProdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MachineProdType",
                table: "m_machine",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MachineProdType",
                table: "m_machine",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
