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
    internal class OrderService : ServiceBase<Order>, IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public async Task<ValidationResponse<IEnumerable<Order>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Order> orders = (await orderRepository.GetAllAsync(cancellationToken)).ToList();

            string message = orders.Any()
                ? "Orders were successfully retrieved."
                : "No orders found.";

            return CreateInfoValidationResponse(orders, message);
        }

        public async Task<ValidationResponse<Order>> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

            return order == null
                ? CreateWarningValidationResponse($"Order with id {orderId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(order, $"Order with id {orderId} was successfully retrieved.");
        }

        public async Task<ValidationResponse<Order>> AddAsync(Order order, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var orderProduct in order.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {orderProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                //var storedProductSize = await productRepository.GetProductSizeAsync(orderProduct.ProductId, orderProduct.SizeId, cancellationToken);

                //if (storedProductSize == null)
                //{
                //    return CreateWarningValidationResponse($"Size with id {orderProduct.SizeId} is not found.", ValidationStatusType.NotFound);
                //}

                //storedProductSize.Quantity -= orderProduct.Quantity;
                //await productRepository.UpdateProductSize(storedProductSize, cancellationToken);
            }

            var addedOrder = await orderRepository.AddAsync(order, cancellationToken);

            return CreateInfoValidationResponse(addedOrder, $"Order with id {addedOrder.Id} was successfully added.");
        }

        public Task<ValidationResponse<Order>> UpdateAsync(Order category, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResponse<Order>> DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}