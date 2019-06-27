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

        public async Task<ValidationResponse<IEnumerable<Order>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Order> orders = (await orderRepository.GetAllAsync(cancellationToken))
                .OrderBy(order => order.OrderDate)
                .ToList();

            var message = orders.Any()
                ? "Orders were successfully retrieved."
                : "No orders found.";

            return CreateInfoValidationResponse(orders, message);
        }

        public async Task<ValidationResponse<Order>> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return CreateWarningValidationResponse($"Order with id {orderId} is not found.", ValidationStatusType.NotFound);
            }

            order.Products = order.Products.OrderByDescending(p => p.Product.Category.IsAdditional).ToList();
            return CreateInfoValidationResponse(order, $"Order with id {orderId} was successfully retrieved.");
        }

        public async Task<ValidationResponse<Order>> AddAsync(Order order, CancellationToken cancellationToken = default(CancellationToken))
        {
            var customerValidationResponse = await orderCustomerProvider.GetCustomerForNewOrderAsync(order.Customer, cancellationToken);
            if (!customerValidationResponse.IsValid)
            {
                return CreateWarningValidationResponse(customerValidationResponse.ValidationResult.Message);
            }
            
            order.CustomerId = customerValidationResponse.Result.Id;

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

            var customerValidationResponse = await orderCustomerProvider.GetCustomerForExistingOrderAsync(storedOrder.CustomerId, order.Customer, cancellationToken);
            if (!customerValidationResponse.IsValid)
            {
                return CreateWarningValidationResponse(customerValidationResponse.ValidationResult.Message);
            }
            
            order.CustomerId = customerValidationResponse.Result.Id;

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

            foreach (var removedProduct in storedOrder.Products.Except(order.Products, new OrderProductComparer()))
            {
                var storedProduct = await productRepository.GetByIdAsync(removedProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {removedProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                storedProduct.Quantity += removedProduct.Quantity;
                productRepository.Update(storedProduct);
            }
            
            var updatedOrder = orderRepository.Update(order);
            await storage.SaveAsync(cancellationToken);

            //if (oldCustomerId != order.CustomerId)
            //{
            //    await customerRepository.GetByIdAsync(oldCustomerId, cancellationToken);
            //    await storage.SaveAsync(cancellationToken);
            //}

            return CreateInfoValidationResponse(updatedOrder, $"Order with id {order.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<Order>> DeleteAsync(Guid orderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

            if (order == null)
            {
                return CreateWarningValidationResponse($"Order with id {orderId} is not found.", ValidationStatusType.NotFound);
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

        public async Task<ValidationResponse<ExportedFile>> ExportAsync(string title, IEnumerable<Guid> orderIds, CancellationToken cancellationToken = default(CancellationToken))
        {
            var orders = await orderRepository.GetForExportAsync(order => orderIds.Contains(order.Id), cancellationToken);

            var fileContent = excelService.ExportOrders(title, orders);
            var exportedFile = new ExportedFile(ExportFileName, ExportContentType, fileContent);

            return CreateInfoValidationResponse(exportedFile, "The orders were exported successfully.");
        }

        private void ClearEmptyCustomers()
        {

        }

        private async Task<ValidationResponse<Customer>> GetCustomerForExistingOrder(Guid storedCustomerId, Customer customer, CancellationToken cancellationToken)
        {
            var storedCustomer = await customerRepository.GetByIdAsync(storedCustomerId, cancellationToken);
            var samePhoneCustomer = await customerRepository.GetByPhoneAsync(customer.Phone, cancellationToken);

            if (samePhoneCustomer == null)
            {
                string message;

                if (storedCustomer.Orders.Count == 1)
                {
                    customer.Id = storedCustomerId;
                    message = $"Existing customer {customer.FullName} was updated";
                }
                else
                {
                    // customer.Id = guidProvider.NewGuid();
                    customerRepository.Add(customer);
                    message = $"New customer {customer.FullName} was added";
                }

                return CreateInfoValidationResponse(customer, message);
            }

            // old customer
            if (storedCustomer.Id == samePhoneCustomer.Id)
            {
                // if Orders.Count > 1 and first name or last name was changed
                if (storedCustomer.Orders.Count > 1 && (storedCustomer.FirstName != customer.FirstName ||
                                                        storedCustomer.LastName != customer.LastName))
                {
                    return CreateWarningValidationResponse<Customer>($"Phone number {customer.Phone} is already used for the customer {storedCustomer.FullName}");
                }

                customer.Id = storedCustomerId;
                customerRepository.Update(customer);
                return CreateInfoValidationResponse(customer, $"Existing customer {customer.FullName} was added");
            }
            else // other existing customer
            {
                if (samePhoneCustomer.FirstName == customer.FirstName && samePhoneCustomer.LastName == customer.LastName)
                {
                    //if (storedCustomer.Orders.Count == 1)
                    //{
                    //    storedCustomer.Orders = null;
                    //    customerRepository.Delete(storedCustomer);
                    //}

                    customer.Id = samePhoneCustomer.Id;
                    return CreateInfoValidationResponse(customer, $"Other existing customer {samePhoneCustomer.FullName} was added");
                }
                else
                {
                    return CreateWarningValidationResponse<Customer>($"Phone number {customer.Phone} is already used for the customer {samePhoneCustomer.FullName}");
                }
            }
        }
    }
}