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
            var operationResult = await supplierService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<SearchSuppliersResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("{id:guid}/describe")]
        public async Task<IActionResult> Describe(Guid id)
        {
            var operationResult = await supplierService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<DescribeSupplierResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSupplierRequestModel supplierRequest)
        {
            var supplier = mapper.Map<Supplier>(supplierRequest);

            var operationResult = await supplierService.AddAsync(supplier, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateSupplierRequestModel supplierRequest)
        {
            var supplier = mapper.Map<Supplier>(supplierRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var operationResult = await supplierService.UpdateAsync(supplier, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var operationResult = await supplierService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }
    }
}