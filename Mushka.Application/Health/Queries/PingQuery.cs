using Mediator;

namespace Mushka.Application.Health.Queries;

public sealed record PingQuery : IQuery<HealthReadModel>;
