using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelativisticCalculator.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStarDistanceName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Distance",
                table: "Stars",
                newName: "DistanceLy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DistanceLy",
                table: "Stars",
                newName: "Distance");
        }
    }
}
