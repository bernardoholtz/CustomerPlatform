using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Validators;
using CustomerPlatform.Domain.Enums;
using CustomerPlatform.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CustomerPlatform.Tests.Validators;

public class CreateCustomerCommandValidatorTests
{
    private readonly CreateCustomerCommandValidator _validator;

    public CreateCustomerCommandValidatorTests()
    {
        _validator = new CreateCustomerCommandValidator();
    }

    [Fact]
    public void Validate_DeveSerValido_QuandoPessoaFisicaComDadosValidos()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
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
        var command = new CreateCustomerCommand
        {
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
        var command = new CreateCustomerCommand
        {
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
        var command = new CreateCustomerCommand
        {
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
        var command = new CreateCustomerCommand
        {
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
        var command = new CreateCustomerCommand
        {
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
    public void Validate_DeveSerInvalido_QuandoCPFTamanhoInvalidoParaPessoaFisica()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "123456789", // Tamanho inválido
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
    public void Validate_DeveSerInvalido_QuandoDataNascimentoNulaParaPessoaFisica()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "52998224725",
            DataNascimento = null,
            Email = "joao@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DataNascimento");
    }

    [Fact]
    public void Validate_DeveSerInvalido_QuandoRazaoSocialVaziaParaPessoaJuridica()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
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

    [Fact]
    public void Validate_DeveSerInvalido_QuandoCNPJVazioParaPessoaJuridica()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            TipoCliente = TipoCliente.PessoaJuridica,
            RazaoSocial = "Empresa LTDA",
            NomeFantasia = "Empresa",
            CNPJ = "",
            Email = "empresa@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CNPJ");
    }

    [Fact]
    public void Validate_DeveSerInvalido_QuandoCNPJTamanhoInvalidoParaPessoaJuridica()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            TipoCliente = TipoCliente.PessoaJuridica,
            RazaoSocial = "Empresa LTDA",
            NomeFantasia = "Empresa",
            CNPJ = "1234567890123", // Tamanho inválido
            Email = "empresa@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CNPJ");
    }

    [Fact]
    public void Validate_DeveSerInvalido_QuandoEstadoTamanhoInvalido()
    {
        // Arrange
        var command = new CreateCustomerCommand
        {
            TipoCliente = TipoCliente.PessoaFisica,
            Nome = "João Silva",
            CPF = "52998224725",
            DataNascimento = DateTime.UtcNow.AddYears(-30),
            Email = "joao@email.com",
            Telefone = "2199887766",
            Endereco = new Endereco("Av 7 de Setembro", "4200", "22076090", "Rio de Janeiro", "RJ", "Sala 103")
        };

        // Modificar o estado para tamanho inválido
        var enderecoComEstadoInvalido = new Endereco(
            command.Endereco.Logradouro,
            command.Endereco.Numero,
            command.Endereco.CEP,
            command.Endereco.Cidade,
            "R", // Estado com tamanho inválido
            command.Endereco.Complemento);
        
        command.Endereco = enderecoComEstadoInvalido;

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Endereco.Estado");
    }
}
