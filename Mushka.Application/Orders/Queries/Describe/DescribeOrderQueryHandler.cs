using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Orders.Converters;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Orders.Queries.Describe;

public sealed class DescribeOrderQueryHandler : IQueryHandler<DescribeOrderQuery, DescribeOrderReadModel>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDescribeOrderConverter _converter;

    public DescribeOrderQueryHandler(IStorage storage, IDescribeOrderConverter converter)
    {
        _orderRepository = storage.GetRepository<IOrderRepository>();
        _converter = converter;
    }

    public async ValueTask<DescribeOrderReadModel> Handle(DescribeOrderQuery query, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(query.Id, cancellationToken);

        if (order == null)
        {
            throw new NotFoundException(ValidationErrors.OrderNotFound);
        }

        order.Products = order.Products
            .OrderByDescending(product => product.Product.Category.IsAdditional)
            .ToList();

        return _converter.ToReadModel(order);
    }
}
