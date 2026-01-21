namespace CustomerPlatform.Domain.DTOs
{
    public class CustomerSearchRequest
    {
        public string? Nome { get; set; }
        public string? CPF { get; set; }
        public string? CNPJ { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
