using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VShop.CartApi.Migrations
{
    /// <inheritdoc />
    public partial class rename_cartHeader_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "CartHeaders",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CartHeaders",
                newName: "id");
        }
    }
}
