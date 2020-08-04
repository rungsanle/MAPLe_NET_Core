using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maple2.AdminLTE.Dal.Data.Migrations.MasterDB
{
    public partial class addMenuMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_menu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    nameOption = table.Column<string>(nullable: true),
                    controller = table.Column<string>(nullable: true),
                    action = table.Column<string>(nullable: true),
                    imageClass = table.Column<string>(nullable: true),
                    status = table.Column<bool>(nullable: false),
                    isParent = table.Column<bool>(nullable: false),
                    parentId = table.Column<int>(nullable: true),
                    area = table.Column<string>(nullable: true),
                    menuseq = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_menu", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_menu");
        }
    }
}
