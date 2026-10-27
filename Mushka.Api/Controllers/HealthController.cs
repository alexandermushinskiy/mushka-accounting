using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Application.Health.Queries;

namespace Mushka.Api.Controllers
{
    [Route("api/v1/health")]
    public class HealthController : ControllerBase
    {
        public HealthController(ISender sender)
            : base(sender)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new PingQuery(), cancellationToken);
            return Ok(result);
        }
    }
}
