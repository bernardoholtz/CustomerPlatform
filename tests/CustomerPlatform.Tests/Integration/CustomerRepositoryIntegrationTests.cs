using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.ValueObjects;
using CustomerPlatform.Infrastructure.Contexts;
using CustomerPlatform.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerPlatform.Tests.Integration;

public class CustomerRepositoryIntegrationTests : IDisposable
{
    private readonly CustomerDbContext _context;
    private readonly CustomerRepository _repository;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepositoryIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CustomerDbContext(options);
        _logger = Mock.Of<ILogger<CustomerRepository>>();
        _repository = new CustomerRepository(_context, _logger);
    }

    [Fact]
    public async Task Criar_DeveSalvarClientePessoaFisica_QuandoDadosValidos()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var result = await _repository.Criar(cliente);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        
        var clienteSalvo = await _context.PessoaFisica.FindAsync(cliente.Id);
        clienteSalvo.Should().NotBeNull();
        clienteSalvo!.Nome.Should().Be("João Silva");
        clienteSalvo.CPF.Should().Be("52998224725");
    }

    [Fact]
    public async Task Criar_DeveSalvarClientePessoaJuridica_QuandoDadosValidos()
    {
        // Arrange
        var cliente = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        // Act
        var result = await _repository.Criar(cliente);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        
        var clienteSalvo = await _context.PessoaJuridica.FindAsync(cliente.Id);
        clienteSalvo.Should().NotBeNull();
        clienteSalvo!.RazaoSocial.Should().Be("Empresa LTDA");
        clienteSalvo.CNPJ.Should().Be("67964706000144");
    }

    [Fact]
    public async Task BuscarPorId_DeveRetornarCliente_QuandoClienteExiste()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        await _repository.Criar(cliente);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.BuscarPorId(cliente.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(cliente.Id);
        result.Should().BeOfType<ClientePessoaFisica>();
    }

    [Fact]
    public async Task BuscarPorId_DeveRetornarNull_QuandoClienteNaoExiste()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        // Act
        var result = await _repository.BuscarPorId(idInexistente);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Editar_DeveAtualizarCliente_QuandoClienteExiste()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        await _repository.Criar(cliente);
        await _context.SaveChangesAsync();

        cliente.Atualizar(
            "João Silva Atualizado",
            "52998224725",
            DateTime.UtcNow.AddYears(-25),
            "joao.novo@email.com",
            "2199887767",
            new Endereco("Av Nova", "100", "22000000", "São Paulo", "SP", "Apto 101"));

        // Act
        var result = await _repository.Editar(cliente);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        
        var clienteAtualizado = await _context.PessoaFisica.FindAsync(cliente.Id);
        clienteAtualizado.Should().NotBeNull();
        clienteAtualizado!.Nome.Should().Be("João Silva Atualizado");
        clienteAtualizado.Email.Should().Be("joao.novo@email.com");
    }

    [Fact]
    public async Task BuscaCpfExistente_DeveRetornarCliente_QuandoCPFExiste()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        await _repository.Criar(cliente);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.BuscaCpfExistente("52998224725", Guid.Empty);

        // Assert
        result.Should().NotBeNull();
        result!.CPF.Should().Be("52998224725");
    }

    [Fact]
    public async Task BuscaCpfExistente_DeveRetornarNull_QuandoCPFNaoExiste()
    {
        // Act
        var result = await _repository.BuscaCpfExistente("12345678901", Guid.Empty);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task BuscaCpfExistente_DeveRetornarNull_QuandoCPFExisteMasEhDoMesmoCliente()
    {
        // Arrange
        var cliente = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        await _repository.Criar(cliente);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.BuscaCpfExistente("52998224725", cliente.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task BuscaCnpjExistente_DeveRetornarCliente_QuandoCNPJExiste()
    {
        // Arrange
        var cliente = new ClientePessoaJuridica(
            "Empresa LTDA",
            "Empresa",
            "67964706000144",
            "empresa@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        await _repository.Criar(cliente);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.BuscaCnpjExistente("67964706000144", Guid.Empty);

        // Assert
        result.Should().NotBeNull();
        result!.CNPJ.Should().Be("67964706000144");
    }

    [Fact]
    public async Task BuscaCnpjExistente_DeveRetornarNull_QuandoCNPJNaoExiste()
    {
        // Act
        var result = await _repository.BuscaCnpjExistente("12345678901234", Guid.Empty);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ListaSuspeitosDuplicatas_DeveRetornarLista_QuandoExistemDuplicatas()
    {
        // Arrange
        var cliente1 = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao@email.com",
            "2199887766",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        var cliente2 = new ClientePessoaFisica(
            "João Silva",
            "52998224725",
            DateTime.UtcNow.AddYears(-30),
            "joao2@email.com",
            "2199887767",
            new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103"));

        await _repository.Criar(cliente1);
        await _repository.Criar(cliente2);
        await _context.SaveChangesAsync();

        var duplicata = new SuspeitaDuplicidade
        {
            IdOriginal = cliente1.Id,
            IdSuspeito = cliente2.Id,
            Score = 0.95,
            DetalhesSimilaridade = "CPF e nome iguais",
            DataDeteccao = DateTime.Now
        };

        _context.SuspeitaDuplicidades.Add(duplicata);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ListaSuspeitosDuplicatas(
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow.AddDays(1));

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Score.Should().Be(0.95);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
