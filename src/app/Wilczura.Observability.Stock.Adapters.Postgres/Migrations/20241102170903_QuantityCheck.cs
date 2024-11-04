using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wilczura.Observability.Stock.Adapters.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class QuantityCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "ck_stock_items_quantity",
                table: "stock_items",
                sql: "quantity >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_stock_items_quantity",
                table: "stock_items");
        }
    }
}
