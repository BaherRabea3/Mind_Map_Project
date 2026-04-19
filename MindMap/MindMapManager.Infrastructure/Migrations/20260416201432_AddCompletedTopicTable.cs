using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindMapManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletedTopicTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "completed_topics",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    topic_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_completed_topics", x => new { x.user_id, x.topic_id });
                    table.ForeignKey(
                        name: "completed_user_topic_fk",
                        column: x => x.topic_id,
                        principalTable: "topic",
                        principalColumn: "topic_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "completed_user_user_fk",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_completed_topics_topic_id",
                table: "completed_topics",
                column: "topic_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "completed_topics");
        }
    }
}
