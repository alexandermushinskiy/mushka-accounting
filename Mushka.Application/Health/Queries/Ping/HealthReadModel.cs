namespace Mushka.Application.Health.Queries.Ping;

public record class HealthReadModel(bool IsApiHealthy, bool IsDatabaseHealthy);
