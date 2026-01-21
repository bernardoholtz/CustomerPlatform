namespace CustomerPlatform.Domain.DTOs
{
    /// <summary>
    /// Documento para indexação no Elasticsearch
    /// </summary>
    public class CustomerIndexDocument
    {
        public Guid Id { get; set; }
        public string TipoCliente { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string Documento { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public EnderecoIndexDocument? Endereco { get; set; }
    }

    /// <summary>
    /// Endereço para indexação
    /// </summary>
    public class EnderecoIndexDocument
    {
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
    }
}
