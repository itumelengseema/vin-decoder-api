using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VinDecoder.Api.Migrations
{
    /// <inheritdoc />
    public partial class addSeedManufacturers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Manufacturer",
                table: "Manufacturer");

            migrationBuilder.RenameTable(
                name: "Manufacturer",
                newName: "Manufacturers");

            migrationBuilder.RenameIndex(
                name: "IX_Manufacturer_Wmi",
                table: "Manufacturers",
                newName: "IX_Manufacturers_Wmi");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manufacturers",
                table: "Manufacturers",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "Name", "Wmi" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Honda", "1HG" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Hyundai", "KMH" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Volkswagen", "WVW" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Manufacturers",
                table: "Manufacturers");

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.RenameTable(
                name: "Manufacturers",
                newName: "Manufacturer");

            migrationBuilder.RenameIndex(
                name: "IX_Manufacturers_Wmi",
                table: "Manufacturer",
                newName: "IX_Manufacturer_Wmi");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manufacturer",
                table: "Manufacturer",
                column: "Id");
        }
    }
}
