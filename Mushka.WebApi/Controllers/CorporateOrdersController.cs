using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Entities;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.CorporateOrder;
using Mushka.WebApi.ClientModels.CorporateOrder.GetAll;
using Mushka.WebApi.ClientModels.CorporateOrder.GetById;
using Mushka.WebApi.ClientModels.CorporateOrder.ValidateOrderNumber;
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
        
        [HttpPost("search")]
        public async Task<IActionResult> Search()
        {
            var operationResult = await corporateOrderService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<GetAllResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var operationResult = await corporateOrderService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<CorporateOrderResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CorporateOrderRequestModel corporateOrderRequest)
        {
            var order = mapper.Map<CorporateOrder>(corporateOrderRequest);

            var operationResult = await corporateOrderService.AddAsync(order, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CorporateOrderRequestModel corporateOrderRequest)
        {
            var order = mapper.Map<CorporateOrder>(corporateOrderRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var operationResult = await corporateOrderService.UpdateAsync(order, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var operationResult = await corporateOrderService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPost("validate-number")]
        public async Task<IActionResult> ValidateOrderNumber([FromBody] ValidateCorporateOrderNumberRequestModel requestModel)
        {
            var operationResult = await corporateOrderService.IsNumberExistAsync(requestModel.OrderNumber, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidateCorporateOrderNumberResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }
    }
}