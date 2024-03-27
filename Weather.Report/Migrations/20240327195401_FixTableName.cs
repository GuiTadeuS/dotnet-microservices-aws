using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Report.Migrations
{
    /// <inheritdoc />
    public partial class FixTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvergeHighC",
                table: "weather_report",
                newName: "AverageHighC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AverageHighC",
                table: "weather_report",
                newName: "AvergeHighC");
        }
    }
}
