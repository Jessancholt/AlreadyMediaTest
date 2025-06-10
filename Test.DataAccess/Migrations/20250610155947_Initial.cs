using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscoveryYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoveryYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeteoriteClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeteoriteClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meteorites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    Mass = table.Column<float>(type: "REAL", nullable: false),
                    Fall = table.Column<int>(type: "INTEGER", nullable: false),
                    YearId = table.Column<int>(type: "INTEGER", nullable: false),
                    Reclat = table.Column<float>(type: "REAL", nullable: false),
                    Reclong = table.Column<float>(type: "REAL", nullable: false),
                    UniqueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meteorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meteorites_DiscoveryYears_YearId",
                        column: x => x.YearId,
                        principalTable: "DiscoveryYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Meteorites_MeteoriteClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "MeteoriteClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveryYears_Year",
                table: "DiscoveryYears",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_MeteoriteClasses_Name",
                table: "MeteoriteClasses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_ClassId",
                table: "Meteorites",
                column: "ClassId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_Name",
                table: "Meteorites",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Meteorites_YearId",
                table: "Meteorites",
                column: "YearId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meteorites");

            migrationBuilder.DropTable(
                name: "DiscoveryYears");

            migrationBuilder.DropTable(
                name: "MeteoriteClasses");
        }
    }
}
