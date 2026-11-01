using Mediator;

namespace Mushka.Application.Orders.Queries.ValidateNumber;

public sealed record ValidateOrderNumberQuery(string OrderNumber) : IQuery<ValidateOrderNumberReadModel>;
