using Mediator;

namespace Mushka.Application.CorporateOrders.Queries.ValidateNumber;

public sealed record ValidateCorporateOrderNumberQuery(string OrderNumber) : IQuery<ValidateCorporateOrderNumberReadModel>;
