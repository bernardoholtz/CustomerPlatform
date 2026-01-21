using CustomerPlatform.Domain.DTOs;
using CustomerPlatform.Domain.Entities;
using MediatR;

namespace CustomerPlatform.Application.Commands.DuplicateList
{
    public class DuplicateListCommand : IRequest<List<SuspeitaDuplicidade>>
    {
        public DateTimeOffset DataIni { get; set; }
        public DateTimeOffset DataFim { get; set; }
    }
}
