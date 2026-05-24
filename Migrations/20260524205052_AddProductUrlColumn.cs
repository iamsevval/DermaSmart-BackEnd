using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DermaSmart.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductUrlColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductUrl",
                table: "products",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE products SET ProductUrl = 'https://www.trendyol.com/sr?q=gentle+cleanser' WHERE Id = 1;");
            migrationBuilder.Sql("UPDATE products SET ProductUrl = 'https://www.trendyol.com/sr?q=vitamin+c+serum' WHERE Id = 2;");
            migrationBuilder.Sql("UPDATE products SET ProductUrl = 'https://www.trendyol.com/sr?q=niacinamide+serum' WHERE Id = 3;");
            migrationBuilder.Sql("UPDATE products SET ProductUrl = 'https://www.trendyol.com/sr?q=bha+treatment' WHERE Id = 4;");
            migrationBuilder.Sql("UPDATE products SET ProductUrl = 'https://www.trendyol.com/sr?q=retinol+serum' WHERE Id = 5;");
            migrationBuilder.Sql("UPDATE products SET ProductUrl = 'https://www.trendyol.com/sr?q=aha+exfoliant' WHERE Id = 6;");
            migrationBuilder.Sql("UPDATE products SET ProductUrl = 'https://www.trendyol.com/sr?q=barrier+moisturizer' WHERE Id = 7;");
            migrationBuilder.Sql("UPDATE products SET ProductUrl = 'https://www.trendyol.com/sr?q=night+cream' WHERE Id = 8;");
            migrationBuilder.Sql("UPDATE products SET ProductUrl = 'https://www.trendyol.com/sr?q=daily+sunscreen' WHERE Id = 9;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductUrl",
                table: "products");
        }
    }
}
