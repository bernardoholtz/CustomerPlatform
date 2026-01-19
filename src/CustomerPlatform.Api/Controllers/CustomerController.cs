using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Commands.UpdateCustomer;
using CustomerPlatform.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CustomerPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CreateCustomerHandler _createHandler;
        private readonly UpdateCustomerHandler _updateHandler;

        public CustomerController(CreateCustomerHandler createHandler, 
            UpdateCustomerHandler updateHandler) {
            _createHandler = createHandler;
            _updateHandler = updateHandler;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerRequest request)
        {
            var command = new CreateCustomerCommand(request);
            var customer = await _createHandler.Handle(command);

            return CreatedAtAction(nameof(Post), new { customer }, null);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCustomerRequest request)
        {
            var command = new UpdateCustomerCommand(request);
            var customer = await _updateHandler.Handle(command);

            return CreatedAtAction(nameof(Put), new { customer }, null);
        }

    }
}
