using Mushka.Application.CorporateOrders.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.CorporateOrders.Converters;

public interface IDescribeCorporateOrderConverter
{
    DescribeCorporateOrderReadModel ToReadModel(CorporateOrder corporateOrder);
}
