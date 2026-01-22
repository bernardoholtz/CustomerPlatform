using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Events;

namespace CustomerPlatform.Application.Factories
{
    public static class CustomerEventFactory
    {
        public static CustomerEvent CreateCustomerCreatedEvent(Customer customer)
        {
            return CreateEvent(customer, "ClienteCriado");
        }

        public static CustomerEvent CreateCustomerUpdatedEvent(Customer customer)
        {
            return CreateEvent(customer, "ClienteAtualizado");
        }

        private static CustomerEvent CreateEvent(Customer customer, string eventType)
        {
            var evento = new CustomerEvent
            {
                EventId = Guid.NewGuid(),
                EventType = eventType,
                Timestamp = DateTime.UtcNow,
                Data = new CustomerEventData
                {
                    ClienteId = customer.Id,
                    Telefone = customer.Telefone,
                    Email = customer.Email,
                }
            };

            switch (customer)
            {
                case ClientePessoaFisica pf:
                    evento.Data.TipoCliente = "PF";
                    evento.Data.Nome = pf.Nome;
                    evento.Data.Documento = pf.CPF;
                    break;

                case ClientePessoaJuridica pj:
                    evento.Data.TipoCliente = "PJ";
                    evento.Data.Nome = pj.NomeFantasia;
                    evento.Data.Documento = pj.CNPJ;
                    break;
            }

            return evento;
        }
    }
}
