using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.Controllers;
using Mushka.Application.Health.Queries;
using Xunit;

namespace Mushka.Api.Tests.Controllers
{
    public class HealthControllerTests
    {
        [Fact]
        public async Task Get_SendsPingQuery_AndReturnsReadModel()
        {
            var readModel = new HealthReadModel(true, true);
            var sender = new Mock<ISender>();
            sender
                .Setup(mock => mock.Send(It.IsAny<PingQuery>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<HealthReadModel>(readModel));

            var result = await new HealthController(sender.Object).Get(CancellationToken.None);

            var ok = result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().Be(readModel);
        }
    }
}
