using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VirtualShop.ProductApi.Migrations
{
    /// <inheritdoc />
    public partial class seeding_products : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "Notebook com RTX 4060 e 16GB RAM", "notebook-gamer.jpg", "Notebook Gamer", 4999.99m, 10L },
                    { 2, 2, "Livro sobre boas práticas de programação", "clean-code.jpg", "Clean Code", 89.90m, 50L },
                    { 3, 3, "Camiseta 100% algodão estampa desenvolvedor", "camiseta-dev.jpg", "Camiseta Dev", 59.90m, 100L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);
        }
    }
}
