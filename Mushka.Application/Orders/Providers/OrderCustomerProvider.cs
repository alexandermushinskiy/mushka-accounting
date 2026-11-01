using System;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Application.Exceptions;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Orders.Providers;

public sealed class OrderCustomerProvider : IOrderCustomerProvider
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IGuidProvider _guidProvider;

    public OrderCustomerProvider(IStorage storage, IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
        _customerRepository = storage.GetRepository<ICustomerRepository>();
    }

    public async Task<Customer> GetCustomerForNewOrderAsync(Customer customer, CancellationToken cancellationToken)
    {
        var storedCustomer = await _customerRepository.GetByPhoneAsync(customer.Phone, cancellationToken);
        customer.Id = storedCustomer?.Id ?? _guidProvider.NewGuid();

        if (storedCustomer == null)
        {
            _customerRepository.Add(customer);
            return customer;
        }

        if (storedCustomer.FirstName == customer.FirstName && storedCustomer.LastName == customer.LastName)
        {
            return storedCustomer;
        }

        throw new BusinessRuleException(ValidationErrors.CustomerPhoneNumberExist);
    }

    public async Task<Customer> GetCustomerForExistingOrderAsync(
        Guid storedCustomerId,
        Customer inputCustomer,
        CancellationToken cancellationToken)
    {
        var storedCustomer = await _customerRepository.GetByIdAsync(storedCustomerId, cancellationToken);
        var samePhoneCustomer = await _customerRepository.GetByPhoneAsync(inputCustomer.Phone, cancellationToken);

        if (samePhoneCustomer != null && !IsCustomerValid(storedCustomer, samePhoneCustomer, inputCustomer))
        {
            throw new BusinessRuleException(ValidationErrors.CustomerPhoneNumberExist);
        }

        if (samePhoneCustomer != null)
        {
            inputCustomer.Id = samePhoneCustomer.Id;
        }
        else if (storedCustomer.Orders.Count == 1)
        {
            inputCustomer.Id = storedCustomerId;
        }
        else
        {
            inputCustomer.Id = _guidProvider.NewGuid();
            _customerRepository.Add(inputCustomer);
        }

        return inputCustomer;
    }

    private static bool IsCustomerValid(Customer storedCustomer, Customer samePhoneCustomer, Customer inputCustomer)
    {
        if (storedCustomer.Id == samePhoneCustomer.Id &&
            storedCustomer.Orders.Count > 1 &&
            IsCustomerNamesNotEqual(samePhoneCustomer, inputCustomer))
        {
            return false;
        }

        if (storedCustomer.Id != samePhoneCustomer.Id &&
            IsCustomerNamesNotEqual(samePhoneCustomer, inputCustomer))
        {
            return false;
        }

        return true;
    }

    private static bool IsCustomerNamesNotEqual(Customer customer1, Customer customer2) =>
        customer1.FirstName != customer2.FirstName || customer1.LastName != customer2.LastName;
}
