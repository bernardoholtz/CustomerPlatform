using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Commands.DuplicateList;
using CustomerPlatform.Application.Commands.SearchCustomer;
using CustomerPlatform.Application.Commands.UpdateCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator) {
            _mediator = mediator;
        }
        /// <summary>
        /// Cadastrar cliente
        /// </summary>
        /// <param name="command">Parâmetros para cadastro</param>
        /// <returns>Resultado paginado ordenado por relevância</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerCommand command)
        {
            var customer = await _mediator.Send(command);

            return CreatedAtAction(nameof(Post), new { customer }, null);
        }

        /// <summary>
        /// Editar cliente
        /// </summary>
        /// <param name="command">Parâmetros para edição</param>
        /// <returns>Resultado paginado ordenado por relevância</returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCustomerCommand command)
        {
            var customer = await _mediator.Send(command);

            return CreatedAtAction(nameof(Put), new { customer }, null);
        }

        /// <summary>
        /// Busca avançada de clientes com suporte a fuzzy search, busca exata e parcial
        /// </summary>
        /// <param name="command">Parâmetros de busca</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Resultado paginado ordenado por relevância</returns>
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchCustomerCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                return BadRequest("Command não pode ser nulo");
            }

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Editar cliente
        /// </summary>
        /// <param name="command">Parâmetros de busca</param>
        /// <returns>Lista de duplicatas por relevância</returns>
        [HttpGet("duplicates")]
        public async Task<IActionResult> Get([FromQuery] DuplicateListCommand command, CancellationToken cancellationToken)
        {
    
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
