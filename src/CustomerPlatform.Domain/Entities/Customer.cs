namespace CustomerPlatform.Domain.Entities;

/// <summary>
/// Entidade base para clientes
/// </summary>
public abstract class Customer
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }

    // Endereço
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string CEP { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;

    public abstract string GetDocumento();
    public abstract string GetNome();
}

/// <summary>
/// Cliente Pessoa Física
/// </summary>
public class ClientePessoaFisica : Customer
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }

    public override string GetDocumento() => CPF;
    public override string GetNome() => Nome;
}

/// <summary>
/// Cliente Pessoa Jurídica
/// </summary>
public class ClientePessoaJuridica : Customer
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;

    public override string GetDocumento() => CNPJ;
    public override string GetNome() => RazaoSocial;
}

