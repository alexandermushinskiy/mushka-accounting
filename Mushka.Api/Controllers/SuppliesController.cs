using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Api.ClientModels.Supply;
using Mushka.Api.Converters.Supplies;
using Mushka.Application.Supplies.Commands;
using Mushka.Application.Supplies.Queries.Describe;
using Mushka.Application.Supplies.Queries.Export;
using Mushka.Application.Supplies.Queries.Search;

namespace Mushka.Api.Controllers
{
    [Route("api/v1/supplies")]
    public class SuppliesController : ControllerBase
    {
        private readonly ISupplyRequestConverter _supplyRequestConverter;

        public SuppliesController(ISender sender, ISupplyRequestConverter supplyRequestConverter)
            : base(sender)
        {
            _supplyRequestConverter = supplyRequestConverter;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchSuppliesRequestModel request, CancellationToken cancellationToken)
        {
            var supplies = await sender.Send(new SearchSuppliesQuery(request.SearchKey, request.ProductId), cancellationToken);
            return Ok(supplies);
        }

        [HttpGet("{id:guid}/describe")]
        public async Task<IActionResult> Describe(Guid id, CancellationToken cancellationToken)
        {
            var supply = await sender.Send(new DescribeSupplyQuery(id), cancellationToken);
            return Ok(supply);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SupplyRequestModel request, CancellationToken cancellationToken)
        {
            await sender.Send(_supplyRequestConverter.ToCreateCommand(request), cancellationToken);
            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] SupplyRequestModel request, CancellationToken cancellationToken)
        {
            await sender.Send(_supplyRequestConverter.ToUpdateCommand(id, request), cancellationToken);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await sender.Send(new DeleteSupplyCommand(id), cancellationToken);
            return Ok();
        }

        [HttpPost("export")]
        public async Task<IActionResult> Export([FromBody] ExportSuppliesRequestModel request, CancellationToken cancellationToken)
        {
            var file = await sender.Send(new ExportSuppliesQuery(request.SupplyIds, request.ProductIds), cancellationToken);
            return File(file.Content, file.ContentType, file.Name);
        }
    }
}
