using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Report.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "weather_report",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AvergeHighC = table.Column<decimal>(type: "numeric", nullable: false),
                    AverageLowC = table.Column<decimal>(type: "numeric", nullable: false),
                    RainfallTotalMillimeters = table.Column<decimal>(type: "numeric", nullable: false),
                    Ddd = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather_report", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weather_report");
        }
    }
}
