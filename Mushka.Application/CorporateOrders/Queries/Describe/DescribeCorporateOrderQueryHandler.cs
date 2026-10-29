using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.CorporateOrders.Converters;
using Mushka.Application.Exceptions;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.CorporateOrders.Queries.Describe;

public sealed class DescribeCorporateOrderQueryHandler : IQueryHandler<DescribeCorporateOrderQuery, DescribeCorporateOrderReadModel>
{
    private readonly ICorporateOrderRepository _corporateOrderRepository;
    private readonly IDescribeCorporateOrderConverter _converter;

    public DescribeCorporateOrderQueryHandler(IStorage storage, IDescribeCorporateOrderConverter converter)
    {
        _corporateOrderRepository = storage.GetRepository<ICorporateOrderRepository>();
        _converter = converter;
    }

    public async ValueTask<DescribeCorporateOrderReadModel> Handle(DescribeCorporateOrderQuery query, CancellationToken cancellationToken)
    {
        var corporateOrder = await _corporateOrderRepository.GetByIdAsync(query.Id, cancellationToken);

        if (corporateOrder == null)
        {
            throw new NotFoundException(ValidationErrors.CorporateOrderNotFound);
        }

        return _converter.ToReadModel(corporateOrder);
    }
}
