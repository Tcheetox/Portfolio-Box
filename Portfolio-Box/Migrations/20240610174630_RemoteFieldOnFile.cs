using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio_Box.Migrations
{
    /// <inheritdoc />
    public partial class RemoteFieldOnFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Remote",
                table: "Files",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remote",
                table: "Files");
        }
    }
}
