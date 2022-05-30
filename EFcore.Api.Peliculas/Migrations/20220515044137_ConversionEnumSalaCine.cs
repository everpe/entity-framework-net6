using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFcore.Api.Peliculas.Migrations
{
    public partial class ConversionEnumSalaCine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoSalaCine",
                table: "SalasCine",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "DosDimensiones",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 1,
                column: "TipoSalaCine",
                value: "DosDimensiones");

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 2,
                column: "TipoSalaCine",
                value: "TresDimensiones");

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 3,
                column: "TipoSalaCine",
                value: "DosDimensiones");

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 4,
                column: "TipoSalaCine",
                value: "TresDimensiones");

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 5,
                column: "TipoSalaCine",
                value: "DosDimensiones");

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 6,
                column: "TipoSalaCine",
                value: "TresDimensiones");

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 7,
                column: "TipoSalaCine",
                value: "CXC");

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 8,
                column: "TipoSalaCine",
                value: "DosDimensiones");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TipoSalaCine",
                table: "SalasCine",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "DosDimensiones");

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 1,
                column: "TipoSalaCine",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 2,
                column: "TipoSalaCine",
                value: 2);

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 3,
                column: "TipoSalaCine",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 4,
                column: "TipoSalaCine",
                value: 2);

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 5,
                column: "TipoSalaCine",
                value: 1);

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 6,
                column: "TipoSalaCine",
                value: 2);

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 7,
                column: "TipoSalaCine",
                value: 3);

            migrationBuilder.UpdateData(
                table: "SalasCine",
                keyColumn: "Id",
                keyValue: 8,
                column: "TipoSalaCine",
                value: 1);
        }
    }
}
