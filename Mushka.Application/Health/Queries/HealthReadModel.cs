namespace Mushka.Application.Health.Queries;

public record class HealthReadModel(bool IsApiHealthy, bool IsDatabaseHealthy);
