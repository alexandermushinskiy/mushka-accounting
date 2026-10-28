using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Customers.Converters;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Customers.Queries.GetByName;

public sealed class GetCustomersByNameQueryHandler : IQueryHandler<GetCustomersByNameQuery, GetCustomersByNameReadModel>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IGetCustomersByNameConverter _converter;

    public GetCustomersByNameQueryHandler(IStorage storage, IGetCustomersByNameConverter converter)
    {
        _customerRepository = storage.GetRepository<ICustomerRepository>();
        _converter = converter;
    }

    public async ValueTask<GetCustomersByNameReadModel> Handle(GetCustomersByNameQuery query, CancellationToken cancellationToken)
    {
        var customers = (await _customerRepository.GetByNameAsync(query.Name, cancellationToken)).ToList();

        return _converter.ToReadModel(customers);
    }
}
