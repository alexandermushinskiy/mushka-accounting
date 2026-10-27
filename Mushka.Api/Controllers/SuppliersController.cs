using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Api.ClientModels.Supplier;
using Mushka.Api.Converters.Suppliers;
using Mushka.Application.Suppliers.Commands;
using Mushka.Application.Suppliers.Queries.Describe;
using Mushka.Application.Suppliers.Queries.Search;

namespace Mushka.Api.Controllers
{
    [Route("api/v1/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRequestConverter _supplierRequestConverter;

        public SuppliersController(ISender sender, ISupplierRequestConverter supplierRequestConverter)
            : base(sender)
        {
            _supplierRequestConverter = supplierRequestConverter;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(CancellationToken cancellationToken)
        {
            var suppliers = await sender.Send(new SearchSuppliersQuery(), cancellationToken);
            return Ok(suppliers);
        }

        [HttpGet("{id:guid}/describe")]
        public async Task<IActionResult> Describe(Guid id, CancellationToken cancellationToken)
        {
            var supplier = await sender.Send(new DescribeSupplierQuery(id), cancellationToken);
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSupplierRequestModel request, CancellationToken cancellationToken)
        {
            await sender.Send(_supplierRequestConverter.ToCreateCommand(request), cancellationToken);
            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateSupplierRequestModel request, CancellationToken cancellationToken)
        {
            await sender.Send(_supplierRequestConverter.ToUpdateCommand(id, request), cancellationToken);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await sender.Send(new DeleteSupplierCommand(id), cancellationToken);
            return Ok();
        }
    }
}
