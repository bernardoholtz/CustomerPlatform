using CustomerPlatform.Application.DTO;

namespace CustomerPlatform.Application.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand
    {
        public UpdateCustomerRequest Request { get; }
        public UpdateCustomerCommand(UpdateCustomerRequest request)
        {
            Request = request;
        }
    }
}
