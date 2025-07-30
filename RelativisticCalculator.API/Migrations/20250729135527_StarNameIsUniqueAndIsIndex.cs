using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelativisticCalculator.API.Migrations
{
    /// <inheritdoc />
    public partial class StarNameIsUniqueAndIsIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stars_Name",
                table: "Stars",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stars_Name",
                table: "Stars");
        }
    }
}
