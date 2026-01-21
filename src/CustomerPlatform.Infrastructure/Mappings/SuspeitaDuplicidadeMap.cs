using CustomerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerPlatform.Infrastructure.Mappings;

public class SuspeitaDuplicidadeFisicaMap : IEntityTypeConfiguration<SuspeitaDuplicidade>
{
    public void Configure(EntityTypeBuilder<SuspeitaDuplicidade> builder)
    {
        builder.ToTable("Suspeitas_Duplicidade");

        builder.HasKey(x => x.Id);

        // Mapeamento do JSONB para PostgreSQL (se estiver usando Postgres)
        // Isso permite consultas performáticas dentro do JSON
        builder.Property(x => x.DetalhesSimilaridade)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(x => x.Score)
            .IsRequired();

        builder.Property(x => x.DataDeteccao)
            .IsRequired();

        // Relacionamento com o Cliente Original
        builder.HasOne(x => x.CustomerOriginal)
            .WithMany() // Se você não quiser uma lista de suspeitas dentro do Customer
            .HasForeignKey(x => x.IdOriginal)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com o Cliente Suspeito
        builder.HasOne(x => x.CustomerSuspeito)
            .WithMany()
            .HasForeignKey(x => x.IdSuspeito)
            .OnDelete(DeleteBehavior.Restrict);

        // Índice para acelerar buscas por cliente
        builder.HasIndex(x => x.IdOriginal);
        builder.HasIndex(x => x.IdSuspeito);
    }
}
