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
using Mushka.WebApi.ClientModels.CorporateOrder;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/corporate-orders")]
    public class CorporateOrdersController : ControllerBase
    {
        private readonly ICorporateOrderService corporateOrderService;

        public CorporateOrdersController(
            ICorporateOrderService corporateOrderService,
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.corporateOrderService = corporateOrderService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deliveriesResponse = await corporateOrderService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<CorporateOrder>>, CorporateOrdersListResponseModel>(deliveriesResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var orderResponse = await corporateOrderService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<CorporateOrder>, CorporateOrderResponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CorporateOrderRequestModel corporateOrderRequest)
        {
            var order = mapper.Map<CorporateOrderRequestModel, CorporateOrder>(corporateOrderRequest);

            var orderResponse = await corporateOrderService.AddAsync(order, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<CorporateOrder>, CorporateOrderResponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]CorporateOrderRequestModel corporateOrderRequest)
        {
            var order = mapper.Map<CorporateOrderRequestModel, CorporateOrder>(corporateOrderRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var orderResponse = await corporateOrderService.UpdateAsync(order, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<CorporateOrder>, CorporateOrderResponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var orderResponse = await corporateOrderService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<CorporateOrder>, DeleteResponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("validate-number/{number}")]
        public async Task<IActionResult> ValidateOrderNumber(string number)
        {
            var isExist = await corporateOrderService.IsNumberExistAsync(number, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<bool>, ValidationResponseModel>(isExist);

            return actionResultProvider.Get(clientResponse);
        }
    }
}