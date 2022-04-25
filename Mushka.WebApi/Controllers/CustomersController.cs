using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels.Customer;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomersController(
            ICustomerService customerService,
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.customerService = customerService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter(string name)
        {
            var operationResult = await customerService.GetByNameAsync(name, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<CustomersResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }
    }
}