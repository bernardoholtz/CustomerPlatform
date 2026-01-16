using CustomerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerPlatform.Infrastructure.Mappings;

public class ClientePessoaFisicaMap : IEntityTypeConfiguration<ClientePessoaFisica>
{
    public void Configure(EntityTypeBuilder<ClientePessoaFisica> builder)
    {
        builder.ToTable("Clientes_Pessoa_Fisica");

        builder.Property(pf => pf.Id)
       .HasColumnName("customer_id");

       
        builder.Property(pf => pf.Nome)
            .HasColumnName("nome")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(pf => pf.CPF)
            .HasColumnName("cpf")
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(pf => pf.DataNascimento)
            .HasColumnName("data_nascimento");

        builder.HasIndex(pf => pf.CPF)
            .IsUnique();
    }
}
