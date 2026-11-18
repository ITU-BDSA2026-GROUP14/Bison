using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageLicense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "License",
                table: "Images",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "License",
                table: "Images");
        }
    }
}
