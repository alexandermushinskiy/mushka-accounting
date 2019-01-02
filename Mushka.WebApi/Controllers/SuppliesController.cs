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
using Mushka.WebApi.ClientModels.Supply;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class SuppliesController : ControllerBase
    {
        private readonly ISupplyService deliveryService;

        public SuppliesController(
            ISupplyService deliveryService,
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliesResponse = await deliveryService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<IEnumerable<Supply>>, SuppliesResponseModel>(suppliesResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SupplyRequestModel supplyRequest)
        {
            var supply = mapper.Map<SupplyRequestModel, Supply>(supplyRequest);

            var supplyResponse = await deliveryService.AddAsync(supply, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Supply>, SupplyResponseModel>(supplyResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplyResponse = await deliveryService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<Supply>, DeleteResponseModel>(supplyResponse);

            return actionResultProvider.Get(clientResponse);
        }
    }
}