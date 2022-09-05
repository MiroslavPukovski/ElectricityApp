using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectricityApp.Migrations
{
    public partial class AddElectricityToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Electricities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OBJ_NUMERIS = table.Column<int>(type: "int", nullable: false),
                    TINKLAS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OBT_PAVADINIMAS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OBJ_GV_TIPAS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    P = table.Column<double>(name: "P+", type: "float", nullable: false),
                    PL_T = table.Column<DateTime>(type: "datetime2", nullable: false),
                    P0 = table.Column<double>(name: "P-", type: "float", nullable: false),
                    producedAndConsumed = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Electricities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Electricities");
        }
    }
}
