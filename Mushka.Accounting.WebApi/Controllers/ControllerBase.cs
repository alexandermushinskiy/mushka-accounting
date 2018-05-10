using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Domain.Extensibility.Entities;
using Mushka.Accounting.Service.Extensibility;
using Mushka.Accounting.WebApi.ClientModels;
using Mushka.Accounting.WebApi.Extensibility.Providers;
using Mushka.Accounting.WebApi.Filters;

namespace Mushka.Accounting.WebApi.Controllers
{
    [TypeFilter(typeof(ModelStateValidationFilterAttribute))]
    public abstract class ControllerBase<TEntity> : Controller
        where TEntity : IEntity
    {
        protected readonly ICancellationTokenSourceProvider cancellationTokenSourceProvider;
        protected readonly IActionResultProvider actionResultProvider;
        protected readonly IMapper mapper;
        protected readonly IServiceBase<TEntity> serviceBase;

        protected ControllerBase(
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper,
            IServiceBase<TEntity> serviceBase)
        {
            this.cancellationTokenSourceProvider = cancellationTokenSourceProvider;
            this.mapper = mapper;
            this.actionResultProvider = actionResultProvider;
            this.serviceBase = serviceBase;
        }

        protected async Task<IActionResult> GetAll<TClientResponse>()
        {
            ValidationResponse<IEnumerable<TEntity>> serviceResponse = await serviceBase.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            TClientResponse clientResponse = mapper.Map<ValidationResponse<IEnumerable<TEntity>>, TClientResponse>(serviceResponse);

            return Ok(clientResponse);
        }

        protected async Task<IActionResult> GetById<TClientResponse>(Guid id)
            where TClientResponse : ResourceResponseModelBase
        {
            ValidationResponse<TEntity> serviceResponse = await serviceBase.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            TClientResponse clientResponse = mapper.Map<ValidationResponse<TEntity>, TClientResponse>(serviceResponse);

            return actionResultProvider.Get(clientResponse);
        }

        protected async Task<IActionResult> Post<TClientRequest, TClientResponse>(TClientRequest clientRequest)
            where TClientResponse : ResourceResponseModelBase
        {
            ValidationResponse<TEntity> serviceResponse = await serviceBase.AddAsync(
                mapper.Map<TClientRequest, TEntity>(clientRequest),
                cancellationTokenSourceProvider.Get().Token);

            TClientResponse clientResponse = mapper.Map<ValidationResponse<TEntity>, TClientResponse>(serviceResponse);

            return actionResultProvider.Get(clientResponse, StatusCodes.Status201Created);
        }

        protected async Task<IActionResult> Put<TClientRequest, TClientResponse>(Guid id, TClientRequest categoryRequest)
            where TClientResponse : ResourceResponseModelBase
        {
            TEntity entityToUpdate = mapper.Map<TClientRequest, TEntity>(categoryRequest);
            entityToUpdate.Id = id;

            ValidationResponse<TEntity> view = await serviceBase.UpdateAsync(entityToUpdate, cancellationTokenSourceProvider.Get().Token);
            TClientResponse clientResponse = mapper.Map<ValidationResponse<TEntity>, TClientResponse>(view);

            return actionResultProvider.Get(clientResponse);
        }

        protected async Task<IActionResult> Delete<TClientResponse>(Guid id)
            where TClientResponse : ResourceResponseModelBase
        {
            ValidationResponse<TEntity> serviceResponse = await serviceBase.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            TClientResponse clientResponse = mapper.Map<ValidationResponse<TEntity>, TClientResponse>(serviceResponse);

            return actionResultProvider.Get(clientResponse);
        }
    }
}