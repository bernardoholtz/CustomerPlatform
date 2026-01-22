using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerPlatform.Application.Commands.DuplicateList
{
    public class DuplicateListHandler : IRequestHandler<DuplicateListCommand, List<SuspeitaDuplicidade>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DuplicateListHandler> _logger;

        public DuplicateListHandler(
            IUnitOfWork unitOfWork,
            ILogger<DuplicateListHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<SuspeitaDuplicidade>> Handle(
            DuplicateListCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var dataIni = command.DataIni.ToUniversalTime();
                var dataFim = command.DataFim.ToUniversalTime();

                _logger.LogInformation(
                    "Buscando suspeitas de duplicidade. Período: {DataIni} a {DataFim}",
                    dataIni,
                    dataFim);

                var result = await _unitOfWork.Customers.ListaSuspeitosDuplicatas(
                    dataIni,
                    dataFim);

                _logger.LogInformation(
                    "Encontradas {Count} suspeitas de duplicidade no período",
                    result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao buscar suspeitas de duplicidade. Período: {DataIni} a {DataFim}",
                    command.DataIni,
                    command.DataFim);
                throw;
            }
        }
    }
}
