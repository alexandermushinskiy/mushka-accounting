using Mediator;

namespace Mushka.Application.Customers.Queries.GetByName;

public sealed record GetCustomersByNameQuery(string Name) : IQuery<GetCustomersByNameReadModel>;
