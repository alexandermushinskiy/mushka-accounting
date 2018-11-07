using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Supplier;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliersResponse = await supplierService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<IEnumerable<Supplier>>, SuppliersResponseModel>(suppliersResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var supplierResponse = await supplierService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Supplier>, SupplierResponseModel>(supplierResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SupplierRequestModel supplierRequest)
        {
            var supplier = mapper.Map<SupplierRequestModel, Supplier>(supplierRequest);

            var supplierResponse = await supplierService.AddAsync(supplier, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Supplier>, SupplierResponseModel>(supplierResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplierResponse = await supplierService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Supplier>, DeleteResponseModel>(supplierResponse);

            return actionResultProvider.Get(clientResponse);
        }
    }
}