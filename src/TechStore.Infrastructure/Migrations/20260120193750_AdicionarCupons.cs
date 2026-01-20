using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCupons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CupomId",
                table: "Pedidos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorDesconto",
                table: "Pedidos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Cupom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorMinimoPedido = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UsosMaximos = table.Column<int>(type: "int", nullable: true),
                    UsosAtuais = table.Column<int>(type: "int", nullable: false),
                    DataValidade = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupom", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_CupomId",
                table: "Pedidos",
                column: "CupomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Cupom_CupomId",
                table: "Pedidos",
                column: "CupomId",
                principalTable: "Cupom",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Cupom_CupomId",
                table: "Pedidos");

            migrationBuilder.DropTable(
                name: "Cupom");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_CupomId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "CupomId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "ValorDesconto",
                table: "Pedidos");
        }
    }
}
