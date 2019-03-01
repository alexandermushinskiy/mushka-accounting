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
using Mushka.WebApi.ClientModels.Supply;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class SuppliesController : ControllerBase
    {
        private readonly ISupplyService supplyService;

        public SuppliesController(
            ISupplyService supplyService,
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.supplyService = supplyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliesResponse = await supplyService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<IEnumerable<Supply>>, SuppliesResponseModel>(suppliesResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var supplyResponse = await supplyService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Supply>, SupplyResponseModel>(supplyResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SupplyRequestModel supplyRequest)
        {
            var supply = mapper.Map<SupplyRequestModel, Supply>(supplyRequest);

            var supplyResponse = await supplyService.AddAsync(supply, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Supply>, SupplyResponseModel>(supplyResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]SupplyRequestModel supplyRequest)
        {
            var supply = mapper.Map<SupplyRequestModel, Supply>(supplyRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var supplyResponse = await supplyService.UpdateAsync(supply, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Supply>, SupplyResponseModel>(supplyResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplyResponse = await supplyService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Supply>, DeleteResponseModel>(supplyResponse);

            return actionResultProvider.Get(clientResponse);
        }
    }
}