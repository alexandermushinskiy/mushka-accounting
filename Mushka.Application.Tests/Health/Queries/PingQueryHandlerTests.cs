using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Health.Queries.Ping;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Health.Queries;

public class PingQueryHandlerTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_ReturnsDatabaseConnectionState(bool isDatabaseHealthy)
    {
        var repository = new Mock<IHealthRepository>();
        repository
            .Setup(mock => mock.CanConnectAsync(default))
            .ReturnsAsync(isDatabaseHealthy);

        var actual = await new PingQueryHandler(repository.Object).Handle(new PingQuery(), default);

        actual.Should().Be(new HealthReadModel(true, isDatabaseHealthy));
    }
}
