﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Wilczura.Observability.Stock.Adapters.Postgres;

#nullable disable

namespace Wilczura.Observability.Stock.Adapters.Postgres.Migrations
{
    [DbContext(typeof(StockContext))]
    [Migration("20241102170903_QuantityCheck")]
    partial class QuantityCheck
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Wilczura.Observability.Stock.Adapters.Postgres.Models.Product", b =>
                {
                    b.Property<long>("ProductId")
                        .HasColumnType("bigint")
                        .HasColumnName("product_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("ProductId")
                        .HasName("pk_products");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("Wilczura.Observability.Stock.Adapters.Postgres.Models.StockItem", b =>
                {
                    b.Property<long>("StockItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("stock_item_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("StockItemId"));

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint")
                        .HasColumnName("product_id");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint")
                        .HasColumnName("quantity");

                    b.HasKey("StockItemId")
                        .HasName("pk_stock_items");

                    b.HasIndex("ProductId")
                        .IsUnique()
                        .HasDatabaseName("ix_stock_items_product_id");

                    b.ToTable("stock_items", null, t =>
                        {
                            t.HasCheckConstraint("ck_stock_items_quantity", "quantity >= 0");
                        });
                });

            modelBuilder.Entity("Wilczura.Observability.Stock.Adapters.Postgres.Models.StockItem", b =>
                {
                    b.HasOne("Wilczura.Observability.Stock.Adapters.Postgres.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stock_items_products_product_id");

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}
