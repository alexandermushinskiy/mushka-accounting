using System;
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
using Mushka.WebApi.ClientModels.Exhibition;
using Mushka.WebApi.ClientModels.Exhibition.Describe;
using Mushka.WebApi.ClientModels.Exhibition.GetDefaultProducts;
using Mushka.WebApi.ClientModels.Exhibition.Search;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class ExhibitionsController : ControllerBase
    {
        private readonly IExhibitionService exhibitionService;
        private readonly IDefaultProductsProvider defaultProductsProvider;

        public ExhibitionsController(
            IExhibitionService exhibitionService,
            IDefaultProductsProvider defaultProductsProvider,
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.exhibitionService = exhibitionService;
            this.defaultProductsProvider = defaultProductsProvider;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search()
        {
            var operationResult = await exhibitionService.SearchAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<SearchExhibitionsResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("{id:guid}/describe")]
        public async Task<IActionResult> Describe(Guid id)
        {
            var operationResult = await exhibitionService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<DescribeExhibitionResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("default-products")]
        public async Task<IActionResult> GetDefaultProducts()
        {
            var operationResult = await defaultProductsProvider.GetExhibitionProducts(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<GetDefaultExhibitionProductsResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExhibitionRequestModel exhibitionRequest)
        {
            var exhibition = mapper.Map<Exhibition>(exhibitionRequest);

            var operationResult = await exhibitionService.AddAsync(exhibition, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ExhibitionRequestModel exhibitionRequest)
        {
            var exhibition = mapper.Map<Exhibition>(exhibitionRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var operationResult = await exhibitionService.UpdateAsync(exhibition, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var operationResult = await exhibitionService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }
    }
}