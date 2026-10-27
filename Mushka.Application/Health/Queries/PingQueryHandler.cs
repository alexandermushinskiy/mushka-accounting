using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Health.Queries;

public sealed class PingQueryHandler : IQueryHandler<PingQuery, HealthReadModel>
{
    private readonly IHealthRepository _healthRepository;

    public PingQueryHandler(IHealthRepository healthRepository)
    {
        _healthRepository = healthRepository;
    }

    public async ValueTask<HealthReadModel> Handle(PingQuery query, CancellationToken cancellationToken)
    {
        var isDatabaseHealthy = await _healthRepository.CanConnectAsync(cancellationToken);
        return new HealthReadModel(IsApiHealthy: true, IsDatabaseHealthy: isDatabaseHealthy);
    }
}
