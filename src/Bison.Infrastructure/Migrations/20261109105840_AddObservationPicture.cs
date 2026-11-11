using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.Razor.Initial.Migrations
{
    /// <inheritdoc />
    public partial class AddObservationPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "Posts",
                type: "BLOB",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureContentType",
                table: "Posts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PictureContentType",
                table: "Posts");
        }
    }
}
