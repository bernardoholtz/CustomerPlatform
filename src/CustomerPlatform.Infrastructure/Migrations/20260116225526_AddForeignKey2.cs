using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKey2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_cnpj",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_cpf",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "cnpj",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "cpf",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "data_nascimento",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "nome",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "nome_fantasia",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "razao_social",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "tipo_cliente",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "customers");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "customers",
                newName: "customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customers",
                table: "customers",
                column: "customer_id");

            migrationBuilder.CreateTable(
                name: "clientes_pessoa_fisica",
                columns: table => new
                {
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    data_nascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes_pessoa_fisica", x => x.customer_id);
                    table.ForeignKey(
                        name: "FK_clientes_pessoa_fisica_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientes_pessoa_juridica",
                columns: table => new
                {
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    razao_social = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    nome_fantasia = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes_pessoa_juridica", x => x.customer_id);
                    table.ForeignKey(
                        name: "FK_clientes_pessoa_juridica_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clientes_pessoa_fisica_cpf",
                table: "clientes_pessoa_fisica",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clientes_pessoa_juridica_cnpj",
                table: "clientes_pessoa_juridica",
                column: "cnpj",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clientes_pessoa_fisica");

            migrationBuilder.DropTable(
                name: "clientes_pessoa_juridica");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customers",
                table: "customers");

            migrationBuilder.RenameTable(
                name: "customers",
                newName: "Customers");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "Customers",
                newName: "id");

            migrationBuilder.AddColumn<string>(
                name: "cnpj",
                table: "Customers",
                type: "character varying(14)",
                maxLength: 14,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cpf",
                table: "Customers",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "data_nascimento",
                table: "Customers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nome",
                table: "Customers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nome_fantasia",
                table: "Customers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "razao_social",
                table: "Customers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_cliente",
                table: "Customers",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "id");

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
    }
}
