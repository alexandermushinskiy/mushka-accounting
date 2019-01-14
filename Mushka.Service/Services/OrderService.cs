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
        private readonly ICustomerRepository customerRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.customerRepository = customerRepository;
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
            var storedCustomer = await customerRepository.GetByOrderDetails(order.Customer, cancellationToken);

            if (storedCustomer == null)
            {
                storedCustomer = await customerRepository.AddAsync(order.Customer, cancellationToken);
            }

            foreach (var orderProduct in order.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {orderProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                storedProduct.Quantity -= orderProduct.Quantity;
                await productRepository.UpdateAsync(storedProduct, cancellationToken);
            }

            order.CustomerId = storedCustomer.Id;
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

        public async Task<ValidationResponse<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            var isValid = !await orderRepository.IsExistAsync(order => order.Number == orderNumber, cancellationToken);

            return CreateInfoValidationResponse(isValid, $"Order number {orderNumber} is {(isValid ? "" : "not ")}valid.");
        }
    }
}