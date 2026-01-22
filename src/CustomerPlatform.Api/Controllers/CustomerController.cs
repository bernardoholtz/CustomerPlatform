using CustomerPlatform.Application.Commands.CreateCustomer;
using CustomerPlatform.Application.Commands.DuplicateList;
using CustomerPlatform.Application.Commands.SearchCustomer;
using CustomerPlatform.Application.Commands.UpdateCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(
            IMediator mediator,
            ILogger<CustomerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        /// <summary>
        /// Cadastrar cliente
        /// </summary>
        /// <param name="command">Parâmetros para cadastro</param>
        /// <returns>Resultado paginado ordenado por relevância</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerCommand command)
        {
            try
            {
                _logger.LogInformation(
                    "Recebida requisição para criar cliente. Tipo: {TipoCliente}",
                    command.TipoCliente);

                var customer = await _mediator.Send(command);

                _logger.LogInformation(
                    "Cliente criado com sucesso via API. Id: {CustomerId}",
                    customer);

                return CreatedAtAction(nameof(Post), new { id = customer }, customer);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    "Operação inválida ao criar cliente: {Message}",
                    ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    "Argumento inválido ao criar cliente: {Message}",
                    ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro inesperado ao criar cliente");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Editar cliente
        /// </summary>
        /// <param name="command">Parâmetros para edição</param>
        /// <returns>Resultado paginado ordenado por relevância</returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCustomerCommand command)
        {
            try
            {
                _logger.LogInformation(
                    "Recebida requisição para atualizar cliente. Id: {CustomerId}",
                    command.Id);

                var customer = await _mediator.Send(command);

                _logger.LogInformation(
                    "Cliente atualizado com sucesso via API. Id: {CustomerId}",
                    customer);

                return CreatedAtAction(nameof(Put), new { id = customer }, customer);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    "Operação inválida ao atualizar cliente. Id: {CustomerId}, Message: {Message}",
                    command.Id,
                    ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    "Argumento inválido ao atualizar cliente. Id: {CustomerId}, Message: {Message}",
                    command.Id,
                    ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro inesperado ao atualizar cliente. Id: {CustomerId}",
                    command.Id);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Busca avançada de clientes com suporte a fuzzy search, busca exata e parcial
        /// </summary>
        /// <param name="command">Parâmetros de busca</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Resultado paginado ordenado por relevância</returns>
        [HttpPost("search")]
        public async Task<IActionResult> Search(
            [FromBody] SearchCustomerCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                if (command == null)
                {
                    _logger.LogWarning("Requisição de busca recebida com command nulo");
                    return BadRequest("Command não pode ser nulo");
                }

                _logger.LogInformation(
                    "Recebida requisição de busca. Filtros: Nome={Nome}, CPF={CPF}, CNPJ={CNPJ}",
                    command.Nome,
                    command.CPF,
                    command.CNPJ);

                var result = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation(
                    "Busca concluída via API. Total encontrado: {Total}",
                    result.Total);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro inesperado ao realizar busca");
                return StatusCode(500, new { error = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Listar duplicatas. Formato dos parametros DataIni e DataFim Ex: (2026-01-01)
        /// </summary>
        /// <param name="DataIni">Data Início (2026-01-01)</param>
        /// <param name="DataFim">Data Fim (2026-03-01)</param>
        /// <returns>Lista de duplicatas por relevância. </returns>
        [HttpGet("duplicates")]
        public async Task<IActionResult> Get(
            [FromQuery] DuplicateListCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Recebida requisição para listar duplicatas. Período: {DataIni} a {DataFim}",
                    command.DataIni,
                    command.DataFim);

                var result = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation(
                    "Lista de duplicatas retornada via API. Total: {Count}",
                    result.Count);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro inesperado ao listar duplicatas");
                return StatusCode(500, new { error = "Erro interno do servidor" });
            }
        }
    }
}
