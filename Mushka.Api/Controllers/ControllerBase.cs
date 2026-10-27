using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Api.Filters;

namespace Mushka.Api.Controllers
{
    [TypeFilter(typeof(ModelStateValidationFilterAttribute))]
    public abstract class ControllerBase : Controller
    {
        protected readonly ISender sender;

        protected ControllerBase(ISender sender)
        {
            this.sender = sender;
        }
    }
}
