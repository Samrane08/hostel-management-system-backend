using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class filetabmodified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ByteFile",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Flag",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ByteFile",
                table: "Files",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Files",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Flag",
                table: "Files",
                type: "int",
                nullable: true);
        }
    }
}
