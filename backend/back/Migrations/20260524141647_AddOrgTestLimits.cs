using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mabyWorking.Migrations
{
    /// <inheritdoc />
    public partial class AddOrgTestLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "org_test_limit",
                table: "stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "org_test_passed",
                table: "stats",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "org_test_limit",
                table: "stats");

            migrationBuilder.DropColumn(
                name: "org_test_passed",
                table: "stats");
        }
    }
}
