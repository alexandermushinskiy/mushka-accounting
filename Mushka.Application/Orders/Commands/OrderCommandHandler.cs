using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Orders.Converters;
using Mushka.Application.Orders.Providers;
using Mushka.Domain.Comparers;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Orders.Commands;

public sealed class OrderCommandHandler :
    ICommandHandler<CreateOrderCommand>,
    ICommandHandler<UpdateOrderCommand>,
    ICommandHandler<DeleteOrderCommand>
{
    private readonly IStorage _storage;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderCustomerProvider _orderCustomerProvider;
    private readonly ICreateOrderConverter _createOrderConverter;
    private readonly IUpdateOrderConverter _updateOrderConverter;

    public OrderCommandHandler(
        IStorage storage,
        IOrderCustomerProvider orderCustomerProvider,
        ICreateOrderConverter createOrderConverter,
        IUpdateOrderConverter updateOrderConverter)
    {
        _storage = storage;
        _orderCustomerProvider = orderCustomerProvider;
        _createOrderConverter = createOrderConverter;
        _updateOrderConverter = updateOrderConverter;
        _orderRepository = storage.GetRepository<IOrderRepository>();
        _productRepository = storage.GetRepository<IProductRepository>();
        _customerRepository = storage.GetRepository<ICustomerRepository>();
    }

    public async ValueTask<Unit> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = _createOrderConverter.ToOrder(command);
        var customer = await _orderCustomerProvider.GetCustomerForNewOrderAsync(order.Customer, cancellationToken);
        order.CustomerId = customer.Id;

        foreach (var orderProduct in order.Products)
        {
            var storedProduct = await _productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

            if (storedProduct == null)
            {
                throw new NotFoundException(ValidationErrors.ProductNotFound);
            }

            if (storedProduct.Quantity < orderProduct.Quantity)
            {
                throw new BusinessRuleException(ValidationErrors.ProductNotEnoughInStock);
            }

            storedProduct.Quantity -= orderProduct.Quantity;
            _productRepository.Update(storedProduct);
        }

        _orderRepository.Add(order);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var storedOrder = await _orderRepository.GetByIdAsync(command.Id, cancellationToken);

        if (storedOrder == null)
        {
            throw new NotFoundException(ValidationErrors.OrderNotFound);
        }

        var order = _updateOrderConverter.ToOrder(command);
        var customer = await _orderCustomerProvider.GetCustomerForExistingOrderAsync(
            storedOrder.CustomerId,
            order.Customer,
            cancellationToken);
        order.CustomerId = customer.Id;

        foreach (var orderProduct in order.Products)
        {
            var storedProduct = await _productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

            if (storedProduct == null)
            {
                throw new NotFoundException(ValidationErrors.ProductNotFound);
            }

            var storedOrderQuantity = storedOrder.Products
                .FirstOrDefault(product => product.ProductId == storedProduct.Id)?.Quantity ?? 0;

            if (storedOrderQuantity != orderProduct.Quantity)
            {
                storedProduct.Quantity = storedProduct.Quantity + storedOrderQuantity - orderProduct.Quantity;
                _productRepository.Update(storedProduct);
            }
        }

        foreach (var removedProduct in storedOrder.Products.Except(order.Products, new OrderProductComparer()))
        {
            var storedProduct = await _productRepository.GetByIdAsync(removedProduct.ProductId, cancellationToken);

            if (storedProduct == null)
            {
                throw new NotFoundException(ValidationErrors.ProductNotFound);
            }

            storedProduct.Quantity += removedProduct.Quantity;
            _productRepository.Update(storedProduct);
        }

        _orderRepository.Update(order);
        await _storage.SaveAsync(cancellationToken);

        await TryDeleteCustomerWithoutOrders(storedOrder.CustomerId, order.CustomerId, cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.Id, cancellationToken);

        if (order == null)
        {
            throw new NotFoundException(ValidationErrors.OrderNotFound);
        }

        var storedCustomer = await _customerRepository.GetByPhoneAsync(order.Customer.Phone, cancellationToken);
        if (await _customerRepository.GetOrdersCountAsync(order.CustomerId, cancellationToken) == 1)
        {
            _customerRepository.Delete(storedCustomer);
        }

        foreach (var orderProduct in order.Products)
        {
            var storedProduct = await _productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

            if (storedProduct == null)
            {
                throw new NotFoundException(ValidationErrors.ProductNotFound);
            }

            storedProduct.Quantity += orderProduct.Quantity;
            _productRepository.Update(storedProduct);
        }

        _orderRepository.Delete(order);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task TryDeleteCustomerWithoutOrders(Guid oldCustomerId, Guid updatedCustomerId, CancellationToken cancellationToken)
    {
        if (oldCustomerId == updatedCustomerId)
        {
            return;
        }

        var oldCustomer = await _customerRepository.GetByIdAsync(oldCustomerId, cancellationToken);
        if (oldCustomer.Orders.Count == 0)
        {
            _customerRepository.Delete(oldCustomer);
            await _storage.SaveAsync(cancellationToken);
        }
    }
}
