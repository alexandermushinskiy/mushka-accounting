﻿using System;
using System.Net;
using System.Net.Http;
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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SuppliesFiltersRequestModel suppliesFiltersRequestModel)
        {
            var suppliesFilters = mapper.Map<SuppliesFiltersRequestModel, SuppliesFiltersModel>(suppliesFiltersRequestModel);

            var suppliesResponse = await supplyService.GetByFilterAsync(suppliesFilters, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<ItemsWithTotalCount<Supply>>, DataWithTotalCountResponseModel<SupplyListModel>>(suppliesResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var operationResult = await supplyService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Supply>, SupplyResponseModel>(operationResult);
            
            return actionResultProvider.Get(clientResponse, operationResult.Status);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SupplyRequestModel supplyRequest)
        {
            var supply = mapper.Map<SupplyRequestModel, Supply>(supplyRequest);

            var supplyResponse = await supplyService.AddAsync(supply, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Supply>, SupplyResponseModel>(supplyResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]SupplyRequestModel supplyRequest)
        {
            var supply = mapper.Map<SupplyRequestModel, Supply>(supplyRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var supplyResponse = await supplyService.UpdateAsync(supply, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Supply>, SupplyResponseModel>(supplyResponse);

            return actionResultProvider.Get(clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplyResponse = await supplyService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<OperationResult<Supply>, DeleteResponseModel>(supplyResponse);

            return actionResultProvider.Get(clientResponse);
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