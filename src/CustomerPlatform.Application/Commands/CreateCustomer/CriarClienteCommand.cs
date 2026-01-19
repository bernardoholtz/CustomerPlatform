using CustomerPlatform.Application.DTO;

namespace CustomerPlatform.Application.Commands.CreateCustomer
{
    public class CreateCustomerCommand
    {
        public CreateCustomerRequest Request { get; }
        public CreateCustomerCommand(CreateCustomerRequest request)
        {
            Request = request;
        }
    }
}
