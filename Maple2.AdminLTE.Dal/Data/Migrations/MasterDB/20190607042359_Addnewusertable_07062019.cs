using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maple2.AdminLTE.Dal.Data.Migrations.MasterDB
{
    public partial class Addnewusertable_07062019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameIndex(
            //    name: "IX_m_department_DeptCode_CompanyCode",
            //    table: "m_department",
            //    newName: "IX_DEPTANDCOMP");

            migrationBuilder.DropIndex(
                name: "IX_m_department_DeptCode_CompanyCode",
                table: "m_department"
                );

            migrationBuilder.CreateIndex(
                name: "IX_DEPTANDCOMP",
                table: "m_department",
                columns: new[] { "DeptCode", "CompanyCode" },
                unique: true);

            migrationBuilder.CreateTable(
                name: "m_user",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    UserCode = table.Column<string>(maxLength: 30, nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    EmpCode = table.Column<string>(nullable: true),
                    DeptId = table.Column<int>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true),
                    aspnetuser_Id = table.Column<string>(maxLength: 255, nullable: true),
                    UserImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_user", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_USERANDCOMP",
                table: "m_user",
                columns: new[] { "UserCode", "CompanyCode" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_user");

            migrationBuilder.RenameIndex(
                name: "IX_DEPTANDCOMP",
                table: "m_department",
                newName: "IX_m_department_DeptCode_CompanyCode");
        }
    }
}
