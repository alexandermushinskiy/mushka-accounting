using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels.Analytics;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService analyticsService;

        public AnalyticsController(
            IAnalyticsService analyticsService,
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.analyticsService = analyticsService;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var operationResult = await analyticsService.GetBalance(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<AnalyticsResponseModel<Balance>>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("popular-products")]
        public async Task<IActionResult> GetPopularProducts()
        {
            var operationResult = await analyticsService.GetPopularProducts(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<AnalyticsResponseModel<PopularProduct[]>>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("unpopular-products")]
        public async Task<IActionResult> GetUnpopularProducts()
        {
            var operationResult = await analyticsService.GetUnpopularProducts(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<AnalyticsResponseModel<PopularProduct[]>>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("popular-cities")]
        public async Task<IActionResult> GetPopularCities()
        {
            var operationResult = await analyticsService.GetPopularCities(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<AnalyticsResponseModel<PopularCity[]>>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders(int period)
        {
            var operationResult = await analyticsService.GetOrdersCount(period, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<AnalyticsResponseModel<OrdersCount[]>>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("sold-products")]
        public async Task<IActionResult> GetSoldProducts(int period)
        {
            var operationResult = await analyticsService.GetSoldProductsCount(period, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<AnalyticsResponseModel<SoldProductsCount[]>>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }
    }
}