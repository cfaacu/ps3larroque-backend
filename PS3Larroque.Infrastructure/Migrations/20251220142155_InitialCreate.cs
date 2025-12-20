using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PS3Larroque.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "preventa",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sucursal_id = table.Column<int>(type: "integer", nullable: false),
                    vendedor = table.Column<string>(type: "text", nullable: false),
                    creada_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preventa", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "productos_legacy",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "text", nullable: false),
                    consola = table.Column<string>(type: "text", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    precio_venta_nuevo = table.Column<decimal>(type: "numeric", nullable: false),
                    precio_compra_usado = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos_legacy", x => x.codigo);
                });

            migrationBuilder.CreateTable(
                name: "stock_sucursal",
                columns: table => new
                {
                    codigo = table.Column<string>(type: "text", nullable: false),
                    sucursal = table.Column<string>(type: "text", nullable: false),
                    consola = table.Column<string>(type: "text", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    tipo = table.Column<string>(type: "text", nullable: false),
                    categoria = table.Column<string>(type: "text", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false),
                    precio = table.Column<decimal>(type: "numeric", nullable: false),
                    actualizado_en = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_sucursal", x => new { x.codigo, x.sucursal });
                });

            migrationBuilder.CreateTable(
                name: "preventa_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    preventa_id = table.Column<int>(type: "integer", nullable: false),
                    codigo = table.Column<string>(type: "text", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false),
                    precio_unit = table.Column<decimal>(type: "numeric", nullable: false),
                    MetodoPagoKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preventa_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_preventa_item_preventa_preventa_id",
                        column: x => x.preventa_id,
                        principalTable: "preventa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_preventa_item_preventa_id",
                table: "preventa_item",
                column: "preventa_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "preventa_item");

            migrationBuilder.DropTable(
                name: "productos_legacy");

            migrationBuilder.DropTable(
                name: "stock_sucursal");

            migrationBuilder.DropTable(
                name: "preventa");
        }
    }
}
