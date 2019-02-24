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

        [HttpGet("populars")]
        public async Task<IActionResult> GetPopulars()
        {
            var popularProducts = await analyticsService.GetPopularProducts(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<ValidationResponse<IEnumerable<PopularProduct>>, PopluarProductsResponseModel>(popularProducts);

            return actionResultProvider.Get(clientResponse);
        }
    }
}