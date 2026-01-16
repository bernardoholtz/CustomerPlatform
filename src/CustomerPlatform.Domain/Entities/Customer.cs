namespace CustomerPlatform.Domain.Entities;

/// <summary>
/// Entidade base para clientes
/// </summary>
public abstract class Customer
{
    public Guid Id { get; }
    public string Email { get; }
    public string Telefone { get; }
    public DateTime DataCriacao { get; }
    public DateTime? DataAtualizacao { get; protected set; }
    public Endereco Endereco { get; }

    protected Customer() { }
    protected Customer(
        Guid id,
        string email,
        string telefone,
        Endereco endereco)
    {
        Id = id;
        Email = email;
        Telefone = telefone;
        Endereco = endereco;
        DataCriacao = DateTime.UtcNow;
    }

    public abstract string GetDocumento();
    public abstract string GetNome();
    public abstract bool ValidarDocumento();
}
