using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace mabyWorking.Migrations
{
    /// <inheritdoc />
    public partial class AddOrgTestStatsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "org_test_stats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stats_id = table.Column<long>(type: "bigint", nullable: false),
                    org_test_id = table.Column<long>(type: "bigint", nullable: false),
                    is_passed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_org_test_stats", x => x.id);
                    table.ForeignKey(
                        name: "FK_org_test_stats_org_tests_org_test_id",
                        column: x => x.org_test_id,
                        principalTable: "org_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_org_test_stats_stats_stats_id",
                        column: x => x.stats_id,
                        principalTable: "stats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_org_test_stats_org_test_id",
                table: "org_test_stats",
                column: "org_test_id");

            migrationBuilder.CreateIndex(
                name: "IX_org_test_stats_stats_id",
                table: "org_test_stats",
                column: "stats_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "org_test_stats");
        }
    }
}
