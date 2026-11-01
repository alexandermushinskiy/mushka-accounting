using Mediator;

namespace Mushka.Application.Analytics.Queries.GetOrdersCount;

public sealed record GetOrdersCountQuery(int Period) : IQuery<OrdersCountReadModel>;
