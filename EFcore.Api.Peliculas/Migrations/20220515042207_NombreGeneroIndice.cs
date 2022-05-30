using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFcore.Api.Peliculas.Migrations
{
    public partial class NombreGeneroIndice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Generos_Nombre",
                table: "Generos",
                column: "Nombre",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Generos_Nombre",
                table: "Generos");
        }
    }
}
