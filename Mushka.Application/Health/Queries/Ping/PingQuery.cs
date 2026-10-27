using Mediator;

namespace Mushka.Application.Health.Queries.Ping;

public sealed record PingQuery : IQuery<HealthReadModel>;
