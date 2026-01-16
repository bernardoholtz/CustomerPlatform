using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clientes_pessoa_fisica_customers_customer_id",
                table: "clientes_pessoa_fisica");

            migrationBuilder.DropForeignKey(
                name: "FK_clientes_pessoa_juridica_customers_customer_id",
                table: "clientes_pessoa_juridica");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customers",
                table: "customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_clientes_pessoa_juridica",
                table: "clientes_pessoa_juridica");

            migrationBuilder.DropPrimaryKey(
                name: "PK_clientes_pessoa_fisica",
                table: "clientes_pessoa_fisica");

            migrationBuilder.RenameTable(
                name: "customers",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "clientes_pessoa_juridica",
                newName: "Clientes_Pessoa_Juridica");

            migrationBuilder.RenameTable(
                name: "clientes_pessoa_fisica",
                newName: "Clientes_Pessoa_Fisica");

            migrationBuilder.RenameIndex(
                name: "IX_clientes_pessoa_juridica_cnpj",
                table: "Clientes_Pessoa_Juridica",
                newName: "IX_Clientes_Pessoa_Juridica_cnpj");

            migrationBuilder.RenameIndex(
                name: "IX_clientes_pessoa_fisica_cpf",
                table: "Clientes_Pessoa_Fisica",
                newName: "IX_Clientes_Pessoa_Fisica_cpf");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes_Pessoa_Juridica",
                table: "Clientes_Pessoa_Juridica",
                column: "customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes_Pessoa_Fisica",
                table: "Clientes_Pessoa_Fisica",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Pessoa_Fisica_Customers_customer_id",
                table: "Clientes_Pessoa_Fisica",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Pessoa_Juridica_Customers_customer_id",
                table: "Clientes_Pessoa_Juridica",
                column: "customer_id",
                principalTable: "Customers",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Pessoa_Fisica_Customers_customer_id",
                table: "Clientes_Pessoa_Fisica");

            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Pessoa_Juridica_Customers_customer_id",
                table: "Clientes_Pessoa_Juridica");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes_Pessoa_Juridica",
                table: "Clientes_Pessoa_Juridica");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes_Pessoa_Fisica",
                table: "Clientes_Pessoa_Fisica");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "customers");

            migrationBuilder.RenameTable(
                name: "Clientes_Pessoa_Juridica",
                newName: "clientes_pessoa_juridica");

            migrationBuilder.RenameTable(
                name: "Clientes_Pessoa_Fisica",
                newName: "clientes_pessoa_fisica");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_Pessoa_Juridica_cnpj",
                table: "clientes_pessoa_juridica",
                newName: "IX_clientes_pessoa_juridica_cnpj");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_Pessoa_Fisica_cpf",
                table: "clientes_pessoa_fisica",
                newName: "IX_clientes_pessoa_fisica_cpf");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customers",
                table: "customers",
                column: "customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_clientes_pessoa_juridica",
                table: "clientes_pessoa_juridica",
                column: "customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_clientes_pessoa_fisica",
                table: "clientes_pessoa_fisica",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_clientes_pessoa_fisica_customers_customer_id",
                table: "clientes_pessoa_fisica",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_clientes_pessoa_juridica_customers_customer_id",
                table: "clientes_pessoa_juridica",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
