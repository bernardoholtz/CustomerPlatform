using CustomerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerPlatform.Infra.Mappings;

public class CustomerMap : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.Email)
            .HasColumnName("email")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.Telefone)
            .HasColumnName("telefone")
            .HasMaxLength(20);

        builder.Property(c => c.DataCriacao)
            .HasColumnName("data_criacao")
            .IsRequired();

        builder.Property(c => c.DataAtualizacao)
            .HasColumnName("data_atualizacao");

        // Value Object Endereco (Owned)
        builder.OwnsOne(c => c.Endereco, endereco =>
        {
            endereco.Property(e => e.Logradouro)
                .HasColumnName("logradouro")
                .HasMaxLength(200)
                .IsRequired();

            endereco.Property(e => e.Numero)
                .HasColumnName("numero")
                .HasMaxLength(20)
                .IsRequired();

            endereco.Property(e => e.Complemento)
                .HasColumnName("complemento")
                .HasMaxLength(100);

            endereco.Property(e => e.CEP)
                .HasColumnName("cep")
                .HasMaxLength(8)
                .IsRequired();

            endereco.Property(e => e.Cidade)
                .HasColumnName("cidade")
                .HasMaxLength(100)
                .IsRequired();

            endereco.Property(e => e.Estado)
                .HasColumnName("estado")
                .HasMaxLength(2)
                .IsRequired();
        });
    }
}
