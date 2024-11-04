using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wilczura.Observability.Prices.Adapters.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class StockUniqueProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_stock_items_product_id",
                table: "stock_items");

            migrationBuilder.CreateIndex(
                name: "ix_stock_items_product_id",
                table: "stock_items",
                column: "product_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_stock_items_product_id",
                table: "stock_items");

            migrationBuilder.CreateIndex(
                name: "ix_stock_items_product_id",
                table: "stock_items",
                column: "product_id");
        }
    }
}
