using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineLibrary.Repository.Migrations
{
    /// <inheritdoc />
    public partial class updatedbookmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Available",
                table: "Books",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Available",
                table: "Books",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
