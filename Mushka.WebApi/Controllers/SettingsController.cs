using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IOrderService orderService;

        public SettingsController(
            IOrderService orderService,
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.orderService = orderService;
        }

        [HttpGet("export/orders")]
        public async Task<IActionResult> ExportOrders()
        {
            var orders = orderService.GetAllAsync(cancellationTokenSourceProvider.Get().Token);
            throw new NotImplementedException();
        }
    }
}