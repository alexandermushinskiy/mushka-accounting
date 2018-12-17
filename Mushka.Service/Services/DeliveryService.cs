using System;
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

        public async Task<ValidationResponse<IEnumerable<Delivery>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Delivery> deliveries = (await deliveryRepository.GetAllAsync(cancellationToken))
                .OrderByDescending(delivery => delivery.DeliveryDate)
                .ToList();

            string message = deliveries.Any()
                ? "Deliveries were successfully retrieved."
                : "No deliveries found.";

            return CreateInfoValidationResponse(deliveries, message);
        }

        public async Task<ValidationResponse<Delivery>> GetByIdAsync(Guid deliveryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var delivery = await deliveryRepository.GetByIdAsync(deliveryId, cancellationToken);

            return delivery == null
                ? CreateWarningValidationResponse($"Delivery with id {deliveryId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(delivery, $"Delivery with id {deliveryId} was successfully retrieved.");
        }

        public async Task<ValidationResponse<Delivery>> AddAsync(Delivery delivery, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var deliveryProduct in delivery.Products)
            {
                //var storedProduct = await productRepository.GetByIdAsync(deliveryProduct.ProductId, cancellationToken);

                //if (storedProduct == null)
                //{
                //    return CreateWarningValidationResponse($"Product with id {deliveryProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                //}

                //var storedProductSize = await productRepository.GetProductSizeAsync(deliveryProduct.ProductId, deliveryProduct.SizeId, cancellationToken);

                //if (storedProductSize == null)
                //{
                //    return CreateWarningValidationResponse($"Size with id {deliveryProduct.SizeId} is not found.", ValidationStatusType.NotFound);
                //}
                
                //storedProductSize.Quantity += deliveryProduct.Quantity;
                //await productRepository.UpdateProductSize(storedProductSize, cancellationToken);
            }

            var addedDelivery = await deliveryRepository.AddAsync(delivery, cancellationToken);
            
            return CreateInfoValidationResponse(addedDelivery, $"Delivery with id {addedDelivery.Id} was successfully added.");
        }

        public Task<ValidationResponse<Delivery>> UpdateAsync(Delivery category, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResponse<Delivery>> DeleteAsync(Guid deliveryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Delivery delivery = await deliveryRepository.GetByIdAsync(deliveryId, cancellationToken);

            if (delivery == null)
            {
                return CreateWarningValidationResponse($"Delivery with id {deliveryId} is not found.", ValidationStatusType.NotFound);
            }

            //foreach (var deliveryProduct in delivery.Products)
            //{
            //    var storedProductSize = await productRepository.GetProductSizeAsync(deliveryProduct.ProductId, deliveryProduct.SizeId, cancellationToken);
            //    storedProductSize.Quantity -= deliveryProduct.Quantity;

            //    await productRepository.UpdateProductSize(storedProductSize, cancellationToken);
            //}

            await deliveryRepository.DeleteAsync(delivery, cancellationToken);

            return CreateInfoValidationResponse(delivery, $"Delivery with id {delivery.Id} was successfully deleted.");
        }
    }
}