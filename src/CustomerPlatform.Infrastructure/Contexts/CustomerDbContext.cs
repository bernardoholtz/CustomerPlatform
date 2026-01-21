using CustomerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPlatform.Infrastructure.Contexts;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ClientePessoaFisica> PessoaFisica => Set<ClientePessoaFisica>();
    public DbSet<ClientePessoaJuridica> PessoaJuridica => Set<ClientePessoaJuridica>();
    public DbSet<SuspeitaDuplicidade> SuspeitaDuplicidades => Set<SuspeitaDuplicidade>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
