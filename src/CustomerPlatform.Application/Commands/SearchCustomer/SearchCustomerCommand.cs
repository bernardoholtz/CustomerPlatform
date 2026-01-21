using CustomerPlatform.Domain.DTOs;
using MediatR;

namespace CustomerPlatform.Application.Commands.SearchCustomer
{
    public class SearchCustomerCommand : IRequest<CustomerSearchResponse>
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
