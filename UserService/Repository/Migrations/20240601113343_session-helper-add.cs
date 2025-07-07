using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class sessionhelperadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLoggedin",
                table: "AspNetUserNumericIdentity",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LoginAt",
                table: "AspNetUserNumericIdentity",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "AspNetUserNumericIdentity",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLoggedin",
                table: "AspNetUserNumericIdentity");

            migrationBuilder.DropColumn(
                name: "LoginAt",
                table: "AspNetUserNumericIdentity");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "AspNetUserNumericIdentity");
        }
    }
}
