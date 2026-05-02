using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindMapManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCertificatedCodeToCetificateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificateCode",
                table: "certificate",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateCode",
                table: "certificate");
        }
    }
}
