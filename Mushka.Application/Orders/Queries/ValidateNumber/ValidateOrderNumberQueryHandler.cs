using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Orders.Queries.ValidateNumber;

public sealed class ValidateOrderNumberQueryHandler : IQueryHandler<ValidateOrderNumberQuery, ValidateOrderNumberReadModel>
{
    private readonly IOrderRepository _orderRepository;

    public ValidateOrderNumberQueryHandler(IStorage storage)
    {
        _orderRepository = storage.GetRepository<IOrderRepository>();
    }

    public async ValueTask<ValidateOrderNumberReadModel> Handle(ValidateOrderNumberQuery query, CancellationToken cancellationToken)
    {
        var exists = await _orderRepository.IsExistAsync(order => order.Number == query.OrderNumber, cancellationToken);

        return new ValidateOrderNumberReadModel
        {
            IsValid = !exists
        };
    }
}
