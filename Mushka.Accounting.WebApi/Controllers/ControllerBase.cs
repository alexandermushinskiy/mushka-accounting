using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.WebApi.Extensibility.Providers;
using Mushka.Accounting.WebApi.Filters;

namespace Mushka.Accounting.WebApi.Controllers
{
    [TypeFilter(typeof(ModelStateValidationFilterAttribute))]
    public abstract class ControllerBase : Controller
    {
        protected readonly ICancellationTokenSourceProvider cancellationTokenSourceProvider;
        protected readonly IActionResultProvider actionResultProvider;
        protected readonly IMapper mapper;

        protected ControllerBase(
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
        {
            this.cancellationTokenSourceProvider = cancellationTokenSourceProvider;
            this.mapper = mapper;
            this.actionResultProvider = actionResultProvider;
        }
    }
}