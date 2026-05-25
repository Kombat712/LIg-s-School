using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace mabyWorking.Migrations
{
    /// <inheritdoc />
    public partial class AddOrgTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "org_tests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_org_tests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "org_test_questions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    org_test_id = table.Column<long>(type: "bigint", nullable: false),
                    explanation = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    reward_rings = table.Column<int>(type: "integer", nullable: false),
                    reward_xp = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_org_test_questions", x => x.id);
                    table.ForeignKey(
                        name: "FK_org_test_questions_org_tests_org_test_id",
                        column: x => x.org_test_id,
                        principalTable: "org_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "org_test_answers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_org_test_answers", x => x.id);
                    table.ForeignKey(
                        name: "FK_org_test_answers_org_test_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "org_test_questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_org_test_answers_question_id",
                table: "org_test_answers",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_org_test_questions_org_test_id",
                table: "org_test_questions",
                column: "org_test_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "org_test_answers");

            migrationBuilder.DropTable(
                name: "org_test_questions");

            migrationBuilder.DropTable(
                name: "org_tests");
        }
    }
}
