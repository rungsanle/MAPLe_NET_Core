using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maple2.AdminLTE.Dal.Data.Migrations.TransactionDB
{
    public partial class InitialTransactionDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_arrival_dtl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    ArrivalId = table.Column<int>(nullable: true),
                    LineNo = table.Column<int>(nullable: true),
                    PoLineNo = table.Column<int>(nullable: true),
                    MaterialId = table.Column<int>(nullable: true),
                    MaterialCode = table.Column<string>(nullable: true),
                    MaterialName = table.Column<string>(nullable: true),
                    MaterialDesc = table.Column<string>(nullable: true),
                    OrderQty = table.Column<decimal>(nullable: true),
                    RecvQty = table.Column<decimal>(nullable: true),
                    LotNo = table.Column<string>(nullable: true),
                    LotDate = table.Column<DateTime>(nullable: true),
                    DetailRemark = table.Column<string>(nullable: true),
                    GenLabelStatus = table.Column<string>(nullable: true),
                    NoOfLabel = table.Column<int>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_arrival_dtl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_arrival_dtl_sub",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    ArrivalId = table.Column<int>(nullable: true),
                    DtlLineNo = table.Column<int>(nullable: true),
                    SubLineNo = table.Column<int>(nullable: true),
                    MaterialId = table.Column<int>(nullable: true),
                    NoOfLabel = table.Column<int>(nullable: true),
                    LabelQty = table.Column<decimal>(nullable: true),
                    SubDetail = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_arrival_dtl_sub", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_arrival_hdr",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Is_Active = table.Column<bool>(nullable: false),
                    Created_Date = table.Column<DateTime>(nullable: true),
                    Created_By = table.Column<int>(nullable: true),
                    Updated_Date = table.Column<DateTime>(nullable: true),
                    Updated_By = table.Column<int>(nullable: true),
                    ArrivalNo = table.Column<string>(maxLength: 30, nullable: true),
                    ArrivalDate = table.Column<DateTime>(nullable: false),
                    RawMatTypeId = table.Column<int>(nullable: true),
                    VendorId = table.Column<int>(nullable: true),
                    ArrivalTypeId = table.Column<int>(nullable: true),
                    PurchaseOrderNo = table.Column<string>(maxLength: 30, nullable: true),
                    DocRefNo = table.Column<string>(nullable: true),
                    DocRefDate = table.Column<DateTime>(nullable: true),
                    ArrivalRemark = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_arrival_hdr", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ARRIVALANDCOMP",
                table: "t_arrival_hdr",
                columns: new[] { "ArrivalNo", "CompanyCode" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_arrival_dtl");

            migrationBuilder.DropTable(
                name: "t_arrival_dtl_sub");

            migrationBuilder.DropTable(
                name: "t_arrival_hdr");
        }
    }
}
