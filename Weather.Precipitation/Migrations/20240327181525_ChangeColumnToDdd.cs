using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Precipitation.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnToDdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cep",
                table: "precipitation",
                newName: "Ddd");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ddd",
                table: "precipitation",
                newName: "Cep");
        }
    }
}
