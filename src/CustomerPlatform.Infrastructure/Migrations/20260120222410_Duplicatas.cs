using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Duplicatas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "cep",
                table: "Customers",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);

            migrationBuilder.CreateTable(
                name: "Suspeitas_Duplicidade",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdOriginal = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSuspeito = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    DetalhesSimilaridade = table.Column<string>(type: "jsonb", nullable: false),
                    DataDeteccao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suspeitas_Duplicidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suspeitas_Duplicidade_Customers_IdOriginal",
                        column: x => x.IdOriginal,
                        principalTable: "Customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suspeitas_Duplicidade_Customers_IdSuspeito",
                        column: x => x.IdSuspeito,
                        principalTable: "Customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suspeitas_Duplicidade_IdOriginal",
                table: "Suspeitas_Duplicidade",
                column: "IdOriginal");

            migrationBuilder.CreateIndex(
                name: "IX_Suspeitas_Duplicidade_IdSuspeito",
                table: "Suspeitas_Duplicidade",
                column: "IdSuspeito");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Suspeitas_Duplicidade");

            migrationBuilder.AlterColumn<string>(
                name: "cep",
                table: "Customers",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);
        }
    }
}
