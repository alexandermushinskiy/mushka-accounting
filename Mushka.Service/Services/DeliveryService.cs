using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class DeliveryService : ServiceBase<Delivery>, IDeliveryService
    {
        private readonly IDeliveryRepository deliveryRepository;
        private readonly IProductRepository productRepository;

        public DeliveryService(
            IDeliveryRepository deliveryRepository,
            IProductRepository productRepository,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.deliveryRepository = deliveryRepository;
            this.productRepository = productRepository;
        }

        public async Task<ValidationResponse<Delivery>> AddAsync(Delivery delivery, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var deliveryProduct in delivery.Products)
            {
                var product = await productRepository.GetByIdAsync(deliveryProduct.ProductId, cancellationToken);

                if (product == null)
                {
                    return CreateWarningValidationResponse($"Product with id {deliveryProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }
            }

            var addedDelivery = await deliveryRepository.AddAsync(delivery, cancellationToken);

            return CreateInfoValidationResponse(addedDelivery, $"Delivery {addedDelivery.Id} was successfully created.");
        }

        public async Task<ValidationResponse<IEnumerable<Delivery>>> GetAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Delivery> deliveries = (await deliveryRepository.GetAllAsync(cancellationToken)).ToList();

            string message = deliveries.Any()
                ? "Deliveries were successfully retrieved."
                : "No deliveries found.";

            return CreateInfoValidationResponse(deliveries, message);
        }
    }
}