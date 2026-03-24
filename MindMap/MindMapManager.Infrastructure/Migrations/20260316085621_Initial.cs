using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindMapManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "plan",
                columns: table => new
                {
                    pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__plan__DD37D91A1E7995DF", x => x.pid);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "track",
                columns: table => new
                {
                    track_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    icon_url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__track__24ECC82ED7FB6338", x => x.track_id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    streak = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    last_act_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    reset_pass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    reset_pass_expires = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    pid = table.Column<int>(type: "int", nullable: true),
                    otp_code = table.Column<int>(type: "int", nullable: true),
                    otp_expire = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_verified = table.Column<bool>(type: "bit", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "user_plan_fk",
                        column: x => x.pid,
                        principalTable: "plan",
                        principalColumn: "pid");
                });

            migrationBuilder.CreateTable(
                name: "roadmap",
                columns: table => new
                {
                    rid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    track_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roadmap__C2B7EDE8844A8F08", x => x.rid);
                    table.ForeignKey(
                        name: "track_roadmap_fk",
                        column: x => x.track_id,
                        principalTable: "track",
                        principalColumn: "track_id");
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    not_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    read = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ref_id = table.Column<int>(type: "int", nullable: true),
                    ref_type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__notifica__0E4BE3002DCFD2AB", x => x.not_id);
                    table.ForeignKey(
                        name: "user_notification_fk",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "user_track",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    track_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_track", x => new { x.user_id, x.track_id });
                    table.ForeignKey(
                        name: "user_tarck_track_fk",
                        column: x => x.track_id,
                        principalTable: "track",
                        principalColumn: "track_id");
                    table.ForeignKey(
                        name: "user_tarck_user_fk",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "certificate",
                columns: table => new
                {
                    cert_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cert_url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    rid = table.Column<int>(type: "int", nullable: true),
                    issued_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__certific__024B46EC706205C1", x => x.cert_id);
                    table.ForeignKey(
                        name: "certificate_roadmap_fk",
                        column: x => x.rid,
                        principalTable: "roadmap",
                        principalColumn: "rid");
                    table.ForeignKey(
                        name: "user_certificate_fk",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "level",
                columns: table => new
                {
                    lid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__level__DE105D0723A1DED8", x => x.lid);
                    table.ForeignKey(
                        name: "roadmap_level_fk",
                        column: x => x.rid,
                        principalTable: "roadmap",
                        principalColumn: "rid");
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    rev_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    rate = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    rid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__review__397465D6E4969990", x => x.rev_id);
                    table.ForeignKey(
                        name: "roadmap_review_fk",
                        column: x => x.rid,
                        principalTable: "roadmap",
                        principalColumn: "rid");
                    table.ForeignKey(
                        name: "user_review_fk",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "progress",
                columns: table => new
                {
                    prog_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comp_perc = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    lid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__progress__8C41E25C33043CD1", x => x.prog_id);
                    table.ForeignKey(
                        name: "progress_level_fk",
                        column: x => x.lid,
                        principalTable: "level",
                        principalColumn: "lid");
                    table.ForeignKey(
                        name: "user_progress_fk",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    proj_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    lid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__project__A04A0C2D8FB345F5", x => x.proj_id);
                    table.ForeignKey(
                        name: "project_level_fk",
                        column: x => x.lid,
                        principalTable: "level",
                        principalColumn: "lid");
                });

            migrationBuilder.CreateTable(
                name: "topic",
                columns: table => new
                {
                    topic_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    lid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__topic__D5DAA3E97151A72F", x => x.topic_id);
                    table.ForeignKey(
                        name: "level_topic_fk",
                        column: x => x.lid,
                        principalTable: "level",
                        principalColumn: "lid");
                });

            migrationBuilder.CreateTable(
                name: "user_project",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    proj_id = table.Column<int>(type: "int", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_pro__73BA97CD94F2D5C3", x => new { x.user_id, x.proj_id });
                    table.ForeignKey(
                        name: "FK__user_proj__proj___1CBC4616",
                        column: x => x.proj_id,
                        principalTable: "project",
                        principalColumn: "proj_id");
                    table.ForeignKey(
                        name: "FK__user_proj__user___1BC821DD",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    com_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    topic_id = table.Column<int>(type: "int", nullable: true),
                    parent_com_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__comment__5066738CC2CD55D0", x => x.com_id);
                    table.ForeignKey(
                        name: "comment_topic_fk",
                        column: x => x.topic_id,
                        principalTable: "topic",
                        principalColumn: "topic_id");
                    table.ForeignKey(
                        name: "reply_comment_fk",
                        column: x => x.parent_com_id,
                        principalTable: "comment",
                        principalColumn: "com_id");
                });

            migrationBuilder.CreateTable(
                name: "resource",
                columns: table => new
                {
                    res_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    res_url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    paid = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    topic_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__resource__2090B50D06C7CAAD", x => x.res_id);
                    table.ForeignKey(
                        name: "resource_topic_fk",
                        column: x => x.topic_id,
                        principalTable: "topic",
                        principalColumn: "topic_id");
                });

            migrationBuilder.CreateTable(
                name: "user_comment",
                columns: table => new
                {
                    com_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_com__5066738C8EC67004", x => x.com_id);
                    table.ForeignKey(
                        name: "user_comment_com_fk",
                        column: x => x.com_id,
                        principalTable: "comment",
                        principalColumn: "com_id");
                    table.ForeignKey(
                        name: "user_comment_user_fk",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "bookmark",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    res_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bookmark", x => new { x.user_id, x.res_id });
                    table.ForeignKey(
                        name: "res_bookmark_fk",
                        column: x => x.res_id,
                        principalTable: "resource",
                        principalColumn: "res_id");
                    table.ForeignKey(
                        name: "user_bookmark_fk",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "user_resource",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    res_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_res__7BB73C5F3DCE2088", x => new { x.user_id, x.res_id });
                    table.ForeignKey(
                        name: "FK__user_reso__res_i__18EBB532",
                        column: x => x.res_id,
                        principalTable: "resource",
                        principalColumn: "res_id");
                    table.ForeignKey(
                        name: "FK__user_reso__user___17F790F9",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "idx_bookmark_res_id",
                table: "bookmark",
                column: "res_id");

            migrationBuilder.CreateIndex(
                name: "idx_bookmark_user_id",
                table: "bookmark",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_certificate_rid",
                table: "certificate",
                column: "rid");

            migrationBuilder.CreateIndex(
                name: "idx_certificate_user_id",
                table: "certificate",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__certific__98B88B7F87AEF197",
                table: "certificate",
                column: "cert_url",
                unique: true,
                filter: "[cert_url] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "idx_comment_parent_com_id",
                table: "comment",
                column: "parent_com_id");

            migrationBuilder.CreateIndex(
                name: "idx_comment_topic_id",
                table: "comment",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "idx_level_rid",
                table: "level",
                column: "rid");

            migrationBuilder.CreateIndex(
                name: "idx_notification_read",
                table: "notification",
                column: "read");

            migrationBuilder.CreateIndex(
                name: "idx_notification_user_id",
                table: "notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_progress_lid",
                table: "progress",
                column: "lid");

            migrationBuilder.CreateIndex(
                name: "idx_progress_user_id",
                table: "progress",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "user_progress_level_uq",
                table: "progress",
                columns: new[] { "user_id", "lid" },
                unique: true,
                filter: "[user_id] IS NOT NULL AND [lid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "idx_project_lid",
                table: "project",
                column: "lid");

            migrationBuilder.CreateIndex(
                name: "idx_resource_paid",
                table: "resource",
                column: "paid");

            migrationBuilder.CreateIndex(
                name: "idx_resource_topic_id",
                table: "resource",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "idx_resource_type",
                table: "resource",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "idx_review_rid",
                table: "review",
                column: "rid");

            migrationBuilder.CreateIndex(
                name: "idx_review_user_id",
                table: "review",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "user_review_roadmap_uq",
                table: "review",
                columns: new[] { "user_id", "rid" },
                unique: true,
                filter: "[user_id] IS NOT NULL AND [rid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "idx_roadmap_track_id",
                table: "roadmap",
                column: "track_id");

            migrationBuilder.CreateIndex(
                name: "idx_topic_lid",
                table: "topic",
                column: "lid");

            migrationBuilder.CreateIndex(
                name: "idx_user_comment_com_id",
                table: "user_comment",
                column: "com_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_comment_user_id",
                table: "user_comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_project_proj_id",
                table: "user_project",
                column: "proj_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_project_user_id",
                table: "user_project",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_resource_res_id",
                table: "user_resource",
                column: "res_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_resource_user_id",
                table: "user_resource",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_track_track_id",
                table: "user_track",
                column: "track_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_track_user_id",
                table: "user_track",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_last_act_date",
                table: "Users",
                column: "last_act_date");

            migrationBuilder.CreateIndex(
                name: "idx_user_status",
                table: "Users",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Users_pid",
                table: "Users",
                column: "pid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookmark");

            migrationBuilder.DropTable(
                name: "certificate");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "progress");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "user_comment");

            migrationBuilder.DropTable(
                name: "user_project");

            migrationBuilder.DropTable(
                name: "user_resource");

            migrationBuilder.DropTable(
                name: "user_track");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "project");

            migrationBuilder.DropTable(
                name: "resource");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "topic");

            migrationBuilder.DropTable(
                name: "plan");

            migrationBuilder.DropTable(
                name: "level");

            migrationBuilder.DropTable(
                name: "roadmap");

            migrationBuilder.DropTable(
                name: "track");
        }
    }
}
