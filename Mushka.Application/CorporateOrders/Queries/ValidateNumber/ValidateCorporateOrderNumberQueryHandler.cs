using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.CorporateOrders.Queries.ValidateNumber;

public sealed class ValidateCorporateOrderNumberQueryHandler
    : IQueryHandler<ValidateCorporateOrderNumberQuery, ValidateCorporateOrderNumberReadModel>
{
    private readonly ICorporateOrderRepository _corporateOrderRepository;

    public ValidateCorporateOrderNumberQueryHandler(IStorage storage)
    {
        _corporateOrderRepository = storage.GetRepository<ICorporateOrderRepository>();
    }

    public async ValueTask<ValidateCorporateOrderNumberReadModel> Handle(
        ValidateCorporateOrderNumberQuery query,
        CancellationToken cancellationToken)
    {
        var exists = await _corporateOrderRepository.IsExistAsync(
            order => order.Number == query.OrderNumber,
            cancellationToken);

        return new ValidateCorporateOrderNumberReadModel
        {
            IsValid = !exists
        };
    }
}
