using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindMapManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrolledAtToUserTrackTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_tarck_track_fk",
                table: "user_track");

            migrationBuilder.DropForeignKey(
                name: "user_tarck_user_fk",
                table: "user_track");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_track",
                table: "user_track");

            migrationBuilder.DropIndex(
                name: "idx_user_track_user_id",
                table: "user_track");

            migrationBuilder.RenameIndex(
                name: "idx_user_track_track_id",
                table: "user_track",
                newName: "IX_user_track_track_id");

            migrationBuilder.AddColumn<DateTime>(
                name: "enrolled_at",
                table: "user_track",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_track",
                table: "user_track",
                columns: new[] { "user_id", "track_id" });

            migrationBuilder.AddForeignKey(
                name: "user_track_track_fk",
                table: "user_track",
                column: "track_id",
                principalTable: "track",
                principalColumn: "track_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "user_track_user_fk",
                table: "user_track",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_track_track_fk",
                table: "user_track");

            migrationBuilder.DropForeignKey(
                name: "user_track_user_fk",
                table: "user_track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_track",
                table: "user_track");

            migrationBuilder.DropColumn(
                name: "enrolled_at",
                table: "user_track");

            migrationBuilder.RenameIndex(
                name: "IX_user_track_track_id",
                table: "user_track",
                newName: "idx_user_track_track_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_track",
                table: "user_track",
                columns: new[] { "user_id", "track_id" });

            migrationBuilder.CreateIndex(
                name: "idx_user_track_user_id",
                table: "user_track",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "user_tarck_track_fk",
                table: "user_track",
                column: "track_id",
                principalTable: "track",
                principalColumn: "track_id");

            migrationBuilder.AddForeignKey(
                name: "user_tarck_user_fk",
                table: "user_track",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
