﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class OrderService : ServiceBase<Order>, IOrderService
    {
        private readonly IStorage storage;
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly ICostPriceProvider costPriceProvider;

        public OrderService(
            IStorage storage,
            ICostPriceProvider costPriceProvider,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;
            this.costPriceProvider = costPriceProvider;

            orderRepository = storage.GetRepository<IOrderRepository>();
            productRepository = storage.GetRepository<IProductRepository>();
            customerRepository = storage.GetRepository<ICustomerRepository>();
        }

        public async Task<ValidationResponse<IEnumerable<Order>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Order> orders = (await orderRepository.GetAllAsync(cancellationToken)).ToList();

            var message = orders.Any()
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
                storedCustomer = customerRepository.Add(order.Customer);
            }

            foreach (var orderProduct in order.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {orderProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                if (storedProduct.Quantity < orderProduct.Quantity)
                {
                    return CreateWarningValidationResponse($"Product with id {orderProduct.ProductId} is not enough in stock.");
                }

                storedProduct.Quantity -= orderProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            order.CustomerId = storedCustomer.Id;
            var addedOrder = orderRepository.Add(order);
            
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(addedOrder, $"Order with id {addedOrder.Id} was successfully added.");
        }

        public async Task<ValidationResponse<Order>> UpdateAsync(Order order, CancellationToken cancellationToken = default(CancellationToken))
        {
            var storedOrder = await orderRepository.GetByIdAsync(order.Id, cancellationToken);

            if (storedOrder == null)
            {
                return CreateWarningValidationResponse($"Order with id {order.Id} is not found.", ValidationStatusType.NotFound);
            }

            foreach (var orderProduct in order.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {orderProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                var storedOrderQuantity = storedOrder.Products
                    .FirstOrDefault(p => p.ProductId == storedProduct.Id)?.Quantity ?? 0;

                if (storedOrderQuantity != orderProduct.Quantity)
                {
                    storedProduct.Quantity = storedProduct.Quantity + storedOrderQuantity - orderProduct.Quantity;
                    productRepository.Update(storedProduct);
                }
            }

            order.Customer.Id = storedOrder.Customer.Id;
            order.CustomerId = storedOrder.CustomerId;

            var updatedOrder = orderRepository.Update(order);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(updatedOrder, $"Order with id {order.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<Order>> DeleteAsync(Guid orderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return CreateWarningValidationResponse($"Order with id {orderId} is not found.", ValidationStatusType.NotFound);
            }

            foreach (var orderProduct in order.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {orderProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                storedProduct.Quantity += orderProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            orderRepository.Delete(order);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(order, $"Order with id {order.Id} was successfully deleted.");
        }

        public async Task<ValidationResponse<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            var isValid = !await orderRepository.IsExistAsync(order => order.Number == orderNumber, cancellationToken);

            return CreateInfoValidationResponse(isValid, $"Order number {orderNumber} is {(isValid ? "" : "not ")}valid.");
        }

        public async Task<ValidationResponse<IEnumerable<OrderProduct>>> GetDefaultProducts(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<Guid> productIds = new List<Guid> {
                Guid.Parse("07DF9000-2680-43E7-BA2C-D4F0C48A8CB5"), // открытка
                Guid.Parse("A6BBAD88-3820-4972-8AE9-FC931A62A1E7")  // пакет
            };

            var products = (await productRepository.GetAsync(prod => productIds.Contains(prod.Id) && prod.Quantity > 0, cancellationToken))
                .Select(async prod => new OrderProduct
                {
                    ProductId = prod.Id,
                    Product = prod,
                    Quantity = 1,
                    CostPrice = await costPriceProvider.CalculateAsync(prod.Id, prod.Quantity, cancellationToken)
                })
                .Select(x => x.Result);

            return CreateInfoValidationResponse(products, "Default products were successfully retrieved.");
        }
    }
}