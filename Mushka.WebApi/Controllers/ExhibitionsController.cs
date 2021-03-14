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
            var exhibitionsResponse = await exhibitionService.SearchAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<Exhibition>>, SearchExhibitionsResponseModel>(exhibitionsResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}/describe")]
        public async Task<IActionResult> Describe(Guid id)
        {
            var exhibitionResponse = await exhibitionService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Exhibition>, DescribeExhibitionResponseModel>(exhibitionResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("default-products")]
        public async Task<IActionResult> GetDefaultProducts()
        {
            var productsResponse = await defaultProductsProvider.GetExhibitionProducts(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<IEnumerable<ExhibitionProduct>>, GetDefaultExhibitionProductsResponseModel>(productsResponse);

            return actionResultProvider.Get(clientResponse);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExhibitionRequestModel exhibitionRequest)
        {
            var exhibition = mapper.Map<ExhibitionRequestModel, Exhibition>(exhibitionRequest);

            var exhibitionResponse = await exhibitionService.AddAsync(exhibition, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Exhibition>, EmptyResponseModel>(exhibitionResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ExhibitionRequestModel exhibitionRequest)
        {
            var exhibition = mapper.Map<ExhibitionRequestModel, Exhibition>(exhibitionRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var exhibitionResponse = await exhibitionService.UpdateAsync(exhibition, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Exhibition>, EmptyResponseModel>(exhibitionResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exhibitionResponse = await exhibitionService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Exhibition>, DeleteResponseModel>(exhibitionResponse);

            return actionResultProvider.Get(clientResponse);
        }
    }
}