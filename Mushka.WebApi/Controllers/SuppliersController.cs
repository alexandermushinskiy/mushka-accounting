using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Entities;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Supplier;
using Mushka.WebApi.ClientModels.Supplier.Describe;
using Mushka.WebApi.ClientModels.Supplier.Modify;
using Mushka.WebApi.ClientModels.Supplier.Search;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService supplierService;

        public SuppliersController(
            ISupplierService supplierService,
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.supplierService = supplierService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search()
        {
            var suppliersResponse = await supplierService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<Supplier>>, SearchSuppliersResponseModel>(suppliersResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}/describe")]
        public async Task<IActionResult> Describe(Guid id)
        {
            var supplierResponse = await supplierService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Supplier>, DescribeSupplierResponseModel>(supplierResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSupplierRequestModel supplierRequest)
        {
            var supplier = mapper.Map<CreateSupplierRequestModel, Supplier>(supplierRequest);

            var supplierResponse = await supplierService.AddAsync(supplier, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult, EmptyResponseModel>(supplierResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateSupplierRequestModel supplierRequest)
        {
            var supplier = mapper.Map<UpdateSupplierRequestModel, Supplier>(supplierRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var supplierResponse = await supplierService.UpdateAsync(supplier, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult, EmptyResponseModel>(supplierResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplierResponse = await supplierService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult, EmptyResponseModel>(supplierResponse);

            return actionResultProvider.Get(clientResponse);
        }
    }
}