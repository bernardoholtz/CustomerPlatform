using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    logradouro = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    numero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    tipo_cliente = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    data_nascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    razao_social = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    nome_fantasia = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_cnpj",
                table: "Customers",
                column: "cnpj",
                unique: true,
                filter: "tipo_cliente = 'PJ'");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_cpf",
                table: "Customers",
                column: "cpf",
                unique: true,
                filter: "tipo_cliente = 'PF'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
