using CustomerPlatform.Domain.Entities;
using CustomerPlatform.Domain.Interfaces;
using MediatR;

namespace CustomerPlatform.Application.Commands.DuplicateList
{
    public class DuplicateListHandler : IRequestHandler<DuplicateListCommand, List<SuspeitaDuplicidade>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DuplicateListHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SuspeitaDuplicidade>> Handle(
            DuplicateListCommand command,
            CancellationToken cancellationToken)
        {
            return 
                await _unitOfWork.Customers.ListaSuspeitosDuplicatas(
                    command.DataIni.ToUniversalTime(),
                    command.DataFim.ToUniversalTime());
        }
    }
}
