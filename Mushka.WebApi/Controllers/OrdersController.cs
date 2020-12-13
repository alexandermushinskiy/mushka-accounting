﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Entities;
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Order;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deliveriesResponse = await orderService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<Order>>, OrdersListResponseModel>(deliveriesResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("default-products")]
        public async Task<IActionResult> GetDefaultProducts()
        {
            var productsResponse = await defaultProductsProvider.GetOrderDefaultProducts(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<OrderProduct>>, OrderProductsResponseModel>(productsResponse);

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
        public async Task<IActionResult> Post([FromBody]OrderRequestModel orderRequest)
        {
            var order = mapper.Map<OrderRequestModel, Order>(orderRequest);

            var orderResponse = await orderService.AddAsync(order, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Order>, OrderResponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]OrderRequestModel orderRequest)
        {
            var order = mapper.Map<OrderRequestModel, Order>(orderRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var orderResponse = await orderService.UpdateAsync(order, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Order>, OrderResponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var orderResponse = await orderService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Order>, DeleteResponseModel>(orderResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("validate-number/{number}")]
        public async Task<IActionResult> ValidateOrderNumber(string number)
        {
            var isExist = await orderService.IsNumberExistAsync(number, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<bool>, ValidationResponseModel>(isExist);

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