using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Events;

namespace CustomerPlatform.Application.Factories
{
    /// <summary>
    /// Factory para criação de eventos de Customer
    /// </summary>
    public static class CustomerEventFactory
    {
        /// <summary>
        /// Cria um evento de Customer criado
        /// </summary>
        public static CustomerEvent CreateCustomerCreatedEvent(Customer customer)
        {
            return CreateEvent(customer, "ClienteCriado");
        }

        /// <summary>
        /// Cria um evento de Customer atualizado
        /// </summary>
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
                    ClienteId = customer.Id
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
