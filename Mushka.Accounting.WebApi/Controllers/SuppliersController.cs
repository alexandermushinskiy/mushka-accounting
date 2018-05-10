using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Service.Extensibility;
using Mushka.Accounting.WebApi.ClientModels;
using Mushka.Accounting.WebApi.Extensibility.Providers;

namespace Mushka.Accounting.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class SuppliersController : ControllerBase<Supplier>
    {
        public SuppliersController(
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper,
            ISupplierService supplierService)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper, supplierService)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await GetAll<SuppliersResponseModel>();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return await GetById<SupplierResponseModel>(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SupplierRequestModel supplierRequest)
        {
            return await Post<SupplierRequestModel, SupplierResponseModel>(supplierRequest);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]SupplierRequestModel supplierRequest)
        {
            return await Put<SupplierRequestModel, SupplierResponseModel>(id, supplierRequest);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await Delete<SupplierResponseModel>(id);
        }
    }
}