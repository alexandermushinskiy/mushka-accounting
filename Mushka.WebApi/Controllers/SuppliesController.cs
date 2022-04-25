using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Entities;
using Mushka.Domain.Models;
using Mushka.Service.Extensibility.Dto;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Supply;
using Mushka.WebApi.ClientModels.Supply.Describe;
using Mushka.WebApi.ClientModels.Supply.Search;
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

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchSuppliesRequestModel suppliesFiltersRequestModel)
        {
            var suppliesFilters = mapper.Map<SearchSuppliesRequestModel, SearchSuppliesFilter> (suppliesFiltersRequestModel);

            var operationResult = await supplyService.SearchAsync(suppliesFilters, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<SearchSuppliesResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("{id:guid}/describe")]
        public async Task<IActionResult> Describe(Guid id)
        {
            var operationResult = await supplyService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<DescribeSupplyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SupplyRequestModel supplyRequest)
        {
            var supply = mapper.Map<Supply>(supplyRequest);

            var operationResult = await supplyService.AddAsync(supply, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] SupplyRequestModel supplyRequest)
        {
            var supply = mapper.Map<Supply>(supplyRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var operationResult = await supplyService.UpdateAsync(supply, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var operationResult = await supplyService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPost("export")]
        public async Task<IActionResult> Export([FromBody] ExportRequestModel exportRequestModel)
        {
            var operationResult = await supplyService.ExportAsync(
                exportRequestModel.SupplyIds,
                exportRequestModel.ProductIds,
                cancellationTokenSourceProvider.Get().Token);

            var exportedFile = operationResult.Data;

            return File(exportedFile.FileContent, exportedFile.ContentType, exportedFile.Name);
        }
    }
}