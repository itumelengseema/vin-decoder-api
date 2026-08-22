using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VinDecoder.Api.Migrations
{
    /// <inheritdoc />
    public partial class addVinRegionstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VinRegions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VinRegions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VinRegions",
                columns: new[] { "Id", "Country", "Prefix" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "United States", "1" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "United States", "4" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "United States", "5" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Canada", "2" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Mexico", "3" },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "Japan", "J" },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "South Korea", "K" },
                    { new Guid("10000000-0000-0000-0000-000000000008"), "Germany", "W" },
                    { new Guid("20000000-0000-0000-0000-000000000001"), "South Africa", "AA" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "South Africa", "AB" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "South Africa", "AC" },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "South Africa", "AD" },
                    { new Guid("20000000-0000-0000-0000-000000000005"), "South Africa", "AE" },
                    { new Guid("20000000-0000-0000-0000-000000000006"), "South Africa", "AF" },
                    { new Guid("20000000-0000-0000-0000-000000000007"), "South Africa", "AG" },
                    { new Guid("20000000-0000-0000-0000-000000000008"), "South Africa", "AH" },
                    { new Guid("30000000-0000-0000-0000-000000000001"), "India", "MA" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), "India", "MB" },
                    { new Guid("30000000-0000-0000-0000-000000000003"), "India", "MC" },
                    { new Guid("30000000-0000-0000-0000-000000000004"), "India", "MD" },
                    { new Guid("30000000-0000-0000-0000-000000000005"), "India", "ME" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VinRegions_Prefix",
                table: "VinRegions",
                column: "Prefix",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VinRegions");
        }
    }
}
