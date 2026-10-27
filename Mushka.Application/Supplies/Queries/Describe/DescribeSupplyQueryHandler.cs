using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Supplies.Converters;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Supplies.Queries.Describe;

public sealed class DescribeSupplyQueryHandler : IQueryHandler<DescribeSupplyQuery, DescribeSupplyReadModel>
{
    private readonly ISupplyRepository _supplyRepository;
    private readonly IDescribeSupplyConverter _converter;

    public DescribeSupplyQueryHandler(IStorage storage, IDescribeSupplyConverter converter)
    {
        _supplyRepository = storage.GetRepository<ISupplyRepository>();
        _converter = converter;
    }

    public async ValueTask<DescribeSupplyReadModel> Handle(DescribeSupplyQuery query, CancellationToken cancellationToken)
    {
        var supply = await _supplyRepository.GetByIdAsync(query.Id, cancellationToken);

        if (supply == null)
        {
            throw new NotFoundException(ValidationErrors.SupplyNotFound);
        }

        return _converter.ToReadModel(supply);
    }
}
