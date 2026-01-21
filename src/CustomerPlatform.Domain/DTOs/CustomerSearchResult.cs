namespace CustomerPlatform.Domain.DTOs
{
    /// <summary>
    /// DTO para resultado de busca de cliente
    /// </summary>
    public class CustomerSearchResult
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
        public double Score { get; set; }
        public EnderecoSearchResult? Endereco { get; set; }
    }

    /// <summary>
    /// DTO para endereço no resultado de busca
    /// </summary>
    public class EnderecoSearchResult
    {
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para resposta paginada de busca
    /// </summary>
    public class CustomerSearchResponse
    {
        public List<CustomerSearchResult> Results { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);
    }
}
