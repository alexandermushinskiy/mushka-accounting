using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Application.Customers.Queries.GetByName;

namespace Mushka.Api.Controllers;

[Route("api/v1/customers")]
public class CustomersController : ControllerBase
{
    public CustomersController(ISender sender)
        : base(sender)
    {
    }

    [HttpGet("filter")]
    public async Task<IActionResult> Filter(string name, CancellationToken cancellationToken)
    {
        var customers = await sender.Send(new GetCustomersByNameQuery(name), cancellationToken);
        return Ok(customers);
    }
}
