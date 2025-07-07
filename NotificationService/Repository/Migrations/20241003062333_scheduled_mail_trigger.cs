using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class scheduled_mail_trigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduledMailTrigger",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    To = table.Column<string>(type: "longtext", nullable: true),
                    Cc = table.Column<string>(type: "longtext", nullable: true),
                    Bcc = table.Column<string>(type: "longtext", nullable: true),
                    Subject = table.Column<string>(type: "longtext", nullable: true),
                    Body = table.Column<string>(type: "longtext", nullable: true),
                    Attachment = table.Column<string>(type: "longtext", nullable: true),
                    ScheduledAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SendAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SendStatus = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledMailTrigger", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledMailTrigger");
        }
    }
}
