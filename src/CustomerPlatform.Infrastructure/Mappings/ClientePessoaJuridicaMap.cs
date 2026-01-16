using CustomerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerPlatform.Infrastructure.Mappings;

public class ClientePessoaJuridicaMap : IEntityTypeConfiguration<ClientePessoaJuridica>
{
    public void Configure(EntityTypeBuilder<ClientePessoaJuridica> builder)
    {
        builder.ToTable("Clientes_Pessoa_Juridica");


        builder.Property(pj => pj.Id)
            .HasColumnName("customer_id");

      
        builder.Property(pj => pj.RazaoSocial)
            .HasColumnName("razao_social")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(pj => pj.NomeFantasia)
            .HasColumnName("nome_fantasia")
            .HasMaxLength(200);

        builder.Property(pj => pj.CNPJ)
            .HasColumnName("cnpj")
            .HasMaxLength(14)
            .IsRequired();

        builder.HasIndex(pj => pj.CNPJ)
            .IsUnique();
    }
}
