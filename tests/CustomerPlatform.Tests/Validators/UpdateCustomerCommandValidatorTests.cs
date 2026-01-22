using CustomerPlatform.Application.Commands.UpdateCustomer;
using CustomerPlatform.Application.Validators;
using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CustomerPlatform.Tests.Validators;

public class UpdateCustomerCommandValidatorTests
{
    private readonly UpdateCustomerCommandValidator _validator;

    public UpdateCustomerCommandValidatorTests()
    {
        _validator = new UpdateCustomerCommandValidator();
    }

    [Fact]
    public void Validate_DeveSerValido_QuandoPessoaFisicaComDadosValidos()
    {
        // Arrange
        var command = new UpdateCustomerCommand
        {
            Id = Guid.NewGuid(),
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "joao@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveSerValido_QuandoPessoaJuridicaComDadosValidos()
    {
        // Arrange
        var command = new UpdateCustomerCommand
        {
            Id = Guid.NewGuid(),
            TipoCliente = TipoCliente.PessoaJuridica,
            RazaoSocial = "Empresa LTDA",
            NomeFantasia = "Empresa",
            CNPJ = "67964706000144",
            Email = "empresa@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveSerInvalido_QuandoEmailVazio()
    {
        // Arrange
        var command = new UpdateCustomerCommand
        {
            Id = Guid.NewGuid(),
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_DeveSerInvalido_QuandoEmailInvalido()
    {
        // Arrange
        var command = new UpdateCustomerCommand
        {
            Id = Guid.NewGuid(),
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "email-invalido",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_DeveSerInvalido_QuandoNomeVazioParaPessoaFisica()
    {
        // Arrange
        var command = new UpdateCustomerCommand
        {
            Id = Guid.NewGuid(),
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "joao@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Nome");
    }

    [Fact]
    public void Validate_DeveSerInvalido_QuandoCPFVazioParaPessoaFisica()
    {
        // Arrange
        var command = new UpdateCustomerCommand
        {
            Id = Guid.NewGuid(),
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "joao@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CPF");
    }

    [Fact]
    public void Validate_DeveSerInvalido_QuandoRazaoSocialVaziaParaPessoaJuridica()
    {
        // Arrange
        var command = new UpdateCustomerCommand
        {
            Id = Guid.NewGuid(),
            TipoCliente = TipoCliente.PessoaJuridica,
            RazaoSocial = "",
            NomeFantasia = "Empresa",
            CNPJ = "67964706000144",
            Email = "empresa@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "RazaoSocial");
    }
}
