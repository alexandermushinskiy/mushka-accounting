using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Entities;
using Mushka.Domain.Models;
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Order;
using Mushka.WebApi.ClientModels.Order.Export;
using Mushka.WebApi.ClientModels.Order.GetById;
using Mushka.WebApi.ClientModels.Order.GetDefaultProducts;
using Mushka.WebApi.ClientModels.Order.Search;
using Mushka.WebApi.ClientModels.Order.ValidateOrderNumber;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IDefaultProductsProvider defaultProductsProvider;

        public OrdersController(
            IOrderService orderService,
            IDefaultProductsProvider defaultProductsProvider,
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.orderService = orderService;
            this.defaultProductsProvider = defaultProductsProvider;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchOrdersRequestModel requestModel)
        {
            var searchFilter = mapper.Map<SearchOrdersRequestModel, SearchOrdersFilter>(requestModel);
            var operationResult = await orderService.SearchAsync(searchFilter, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<ItemsList<OrderSummaryDto>>, SearchOrdersResponseModel>(operationResult);
            
            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("default-products")]
        public async Task<IActionResult> GetDefaultProducts()
        {
            var productsResponse = await defaultProductsProvider.GetOrderDefaultProducts(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<OrderProduct>>, OrderDefaultProductResponseModel>(productsResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var orderResponse = await orderService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Order>, OrderResponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequestModel orderRequest)
        {
            var order = mapper.Map<OrderRequestModel, Order>(orderRequest);

            var orderResponse = await orderService.AddAsync(order, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Order>, EmptyReponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] OrderRequestModel orderRequest)
        {
            var order = mapper.Map<OrderRequestModel, Order>(orderRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var orderResponse = await orderService.UpdateAsync(order, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Order>, EmptyReponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var orderResponse = await orderService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Order>, EmptyReponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost("validate-number")]
        public async Task<IActionResult> ValidateOrderNumber([FromBody] ValidateOrderNumberRequestModel requestModel)
        {
            var isExist = await orderService.IsNumberExistAsync(requestModel.OrderNumber, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<bool>, ValidateOrderNumberResponseModel>(isExist);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPost("export")]
        public async Task<IActionResult> Export([FromBody] ExportRequestModel exportRequestModel)
        {
            var exportValidationResponse = await orderService.ExportAsync(
                exportRequestModel.Title,
                exportRequestModel.OrderIds,
                cancellationTokenSourceProvider.Get().Token);

            return File(
                exportValidationResponse.Data.FileContent,
                exportValidationResponse.Data.ContentType,
                exportValidationResponse.Data.Name);
        }
    }
}