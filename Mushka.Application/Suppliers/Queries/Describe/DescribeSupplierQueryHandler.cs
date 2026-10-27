using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Suppliers.Converters;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Suppliers.Queries.Describe;

public sealed class DescribeSupplierQueryHandler : IQueryHandler<DescribeSupplierQuery, DescribeSupplierReadModel>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IDescribeSupplierConverter _converter;

    public DescribeSupplierQueryHandler(IStorage storage, IDescribeSupplierConverter converter)
    {
        _supplierRepository = storage.GetRepository<ISupplierRepository>();
        _converter = converter;
    }

    public async ValueTask<DescribeSupplierReadModel> Handle(DescribeSupplierQuery query, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(query.Id, cancellationToken);

        if (supplier == null)
        {
            throw new NotFoundException(ValidationErrors.SupplierNotFound);
        }

        return _converter.ToReadModel(supplier);
    }
}
