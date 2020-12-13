using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Comparers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Mushka.Service.Extensibility.Dto;
using Mushka.Service.Extensibility.ExternalApps;
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class OrderService : ServiceBase<Order>, IOrderService
    {
        private const string ExportContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const string ExportFileName = "mushka_export_orders.xlsx";

        private readonly IStorage storage;
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IExcelService excelService;
        private readonly IOrderCustomerProvider orderCustomerProvider;

        public OrderService(
            IStorage storage,
            IExcelService excelService,
            IOrderCustomerProvider orderCustomerProvider,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;
            this.excelService = excelService;
            this.orderCustomerProvider = orderCustomerProvider;

            orderRepository = storage.GetRepository<IOrderRepository>();
            productRepository = storage.GetRepository<IProductRepository>();
            customerRepository = storage.GetRepository<ICustomerRepository>();
        }

        public async Task<OperationResult<IEnumerable<Order>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Order> orders = (await orderRepository.GetAllAsync(cancellationToken))
                .OrderByDescending(order => order.OrderDate)
                .ToList();

            return OperationResult<IEnumerable<Order>>.FromResult(orders);
        }

        public async Task<OperationResult<Order>> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return OperationResult<Order>.FromError(ValidationErrors.OrderNotFound, ValidationStatusType.NotFound);
            }

            order.Products = order.Products.OrderByDescending(p => p.Product.Category.IsAdditional).ToList();

            return OperationResult<Order>.FromResult(order);
        }

        public async Task<OperationResult<Order>> AddAsync(Order order, CancellationToken cancellationToken = default(CancellationToken))
        {
            var customerValidationResponse = await orderCustomerProvider.GetCustomerForNewOrderAsync(order.Customer, cancellationToken);
            if (customerValidationResponse.IsFailure)
            {
                return OperationResult<Order>.FromErrors(customerValidationResponse.Errors);
            }
            
            order.CustomerId = customerValidationResponse.Data.Id;

            foreach (var orderProduct in order.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult<Order>.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                if (storedProduct.Quantity < orderProduct.Quantity)
                {
                    return OperationResult<Order>.FromError(ValidationErrors.ProductNotEnoughInStock);
                }

                storedProduct.Quantity -= orderProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            var addedOrder = orderRepository.Add(order);
            await storage.SaveAsync(cancellationToken);
            
            return OperationResult<Order>.FromResult(addedOrder);
        }

        public async Task<OperationResult<Order>> UpdateAsync(Order order, CancellationToken cancellationToken = default(CancellationToken))
        {
            var storedOrder = await orderRepository.GetByIdAsync(order.Id, cancellationToken);

            if (storedOrder == null)
            {
                return OperationResult<Order>.FromError(ValidationErrors.OrderNotFound, ValidationStatusType.NotFound);
            }

            var customerValidationResponse = await orderCustomerProvider.GetCustomerForExistingOrderAsync(storedOrder.CustomerId, order.Customer, cancellationToken);
            if (customerValidationResponse.IsFailure)
            {
                return OperationResult<Order>.FromErrors(customerValidationResponse.Errors);
            }
            
            order.CustomerId = customerValidationResponse.Data.Id;

            foreach (var orderProduct in order.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult<Order>.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                var storedOrderQuantity = storedOrder.Products
                    .FirstOrDefault(p => p.ProductId == storedProduct.Id)?.Quantity ?? 0;

                if (storedOrderQuantity != orderProduct.Quantity)
                {
                    storedProduct.Quantity = storedProduct.Quantity + storedOrderQuantity - orderProduct.Quantity;
                    productRepository.Update(storedProduct);
                }
            }

            foreach (var removedProduct in storedOrder.Products.Except(order.Products, new OrderProductComparer()))
            {
                var storedProduct = await productRepository.GetByIdAsync(removedProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult<Order>.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                storedProduct.Quantity += removedProduct.Quantity;
                productRepository.Update(storedProduct);
            }
            
            var updatedOrder = orderRepository.Update(order);
            await storage.SaveAsync(cancellationToken);

            await TryDeleteCustomerWithoutOrders(storedOrder.CustomerId, order.CustomerId, cancellationToken);

            return OperationResult<Order>.FromResult(updatedOrder);
        }

        public async Task<OperationResult<Order>> DeleteAsync(Guid orderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return OperationResult<Order>.FromError(ValidationErrors.OrderNotFound, ValidationStatusType.NotFound);
            }

            var storedCustomer = await customerRepository.GetByPhoneAsync(order.Customer.Phone, cancellationToken);
            if (await customerRepository.GetOrdersCountAsync(order.CustomerId, cancellationToken) == 1)
            {
                customerRepository.Delete(storedCustomer);
            }

            foreach (var orderProduct in order.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult<Order>.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                storedProduct.Quantity += orderProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            orderRepository.Delete(order);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Order>.FromResult(order);
        }

        public async Task<OperationResult<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            var isValid = !await orderRepository.IsExistAsync(order => order.Number == orderNumber, cancellationToken);

            return OperationResult<bool>.FromResult(isValid);
        }

        public async Task<OperationResult<ExportedFile>> ExportAsync(string title, IEnumerable<Guid> orderIds, CancellationToken cancellationToken = default(CancellationToken))
        {
            var orders = await orderRepository.GetForExportAsync(order => orderIds.Contains(order.Id), cancellationToken);

            var fileContent = excelService.ExportOrders(title, orders);
            var exportedFile = new ExportedFile(ExportFileName, ExportContentType, fileContent);

            return OperationResult<ExportedFile>.FromResult(exportedFile);
        }

        private async Task TryDeleteCustomerWithoutOrders(Guid oldCustomerId, Guid updatedCustomerId, CancellationToken cancellationToken)
        {
            if (oldCustomerId == updatedCustomerId)
            {
                return;
            }

            var oldCustomer = await customerRepository.GetByIdAsync(oldCustomerId, cancellationToken);
            if (oldCustomer.Orders.Count == 0)
            {
                customerRepository.Delete(oldCustomer);
                await storage.SaveAsync(cancellationToken);
            }
        }
    }
}