using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindMapManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addIdToCompletedTopicsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_completed_topics",
                table: "completed_topics");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "completed_topics",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_completed_topics",
                table: "completed_topics",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_completed_topics_user_id",
                table: "completed_topics",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_completed_topics",
                table: "completed_topics");

            migrationBuilder.DropIndex(
                name: "IX_completed_topics_user_id",
                table: "completed_topics");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "completed_topics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_completed_topics",
                table: "completed_topics",
                columns: new[] { "user_id", "topic_id" });
        }
    }
}
