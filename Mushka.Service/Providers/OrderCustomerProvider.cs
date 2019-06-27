using System;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Extensibility.Providers;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Services;

namespace Mushka.Service.Providers
{
    internal class OrderCustomerProvider : ServiceBase, IOrderCustomerProvider
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IGuidProvider guidProvider;

        public OrderCustomerProvider(
            IStorage storage,
            IGuidProvider guidProvider,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.guidProvider = guidProvider;

            customerRepository = storage.GetRepository<ICustomerRepository>();
        }

        public async Task<ValidationResponse<Customer>> GetCustomerForNewOrderAsync(
            Customer customer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var storedCustomer = await customerRepository.GetByPhoneAsync(customer.Phone, cancellationToken);
            customer.Id = storedCustomer?.Id ?? guidProvider.NewGuid();

            if (storedCustomer == null)
            {
                customerRepository.Add(customer);
                return CreateInfoValidationResponse(customer, $"New customer {customer.FullName} was added.");
            }

            if (storedCustomer.FirstName == customer.FirstName && storedCustomer.LastName == customer.LastName)
            {
                return CreateInfoValidationResponse(storedCustomer, $"Existing customer {storedCustomer.FullName} was added.");
            }

            return CreateWarningValidationResponse<Customer>($"Phone number {customer.Phone} is already used for the customer {storedCustomer.FullName}.");
        }

        private static bool IsCustomerValid(Customer storedCustomer, Customer samePhoneCustomer, Customer inputCustomer)
        {
            // 1. if the same (old) customer, which has more than one order and first name or last name was changed
            if (storedCustomer.Id == samePhoneCustomer.Id)
            {
                if (storedCustomer.Orders.Count > 1 && IsCustomerNamesNotEqual(samePhoneCustomer, inputCustomer))
                {
                    return false;
                }
            }

            // 2. if other existing customer, but first name/last name differ
            if (storedCustomer.Id != samePhoneCustomer.Id)
            {
                if (IsCustomerNamesNotEqual(samePhoneCustomer, inputCustomer))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<ValidationResponse<Customer>> GetCustomerForExistingOrderAsync(
            Guid storedCustomerId,
            Customer inputCustomer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var storedCustomer = await customerRepository.GetByIdAsync(storedCustomerId, cancellationToken);
            var samePhoneCustomer = await customerRepository.GetByPhoneAsync(inputCustomer.Phone, cancellationToken);

            // validate
            if (samePhoneCustomer != null && !IsCustomerValid(storedCustomer, samePhoneCustomer, inputCustomer))
            {
                return CreateWarningValidationResponse<Customer>(
                    $"Phone number {inputCustomer.Phone} is already used for the customer {samePhoneCustomer.FullName}.");
            }

            if (samePhoneCustomer != null)
            {
                inputCustomer.Id = samePhoneCustomer.Id;
            }
            else
            {
                if (storedCustomer.Orders.Count == 1)
                {
                    inputCustomer.Id = storedCustomerId;
                }
                else
                {
                    inputCustomer.Id = guidProvider.NewGuid();
                    customerRepository.Add(inputCustomer);
                }
            }
            
            return CreateInfoValidationResponse(inputCustomer, $"Customer with name {inputCustomer.FullName} is used.");





            //// old customer
            //if (storedCustomer.Id == samePhoneCustomer.Id)
            //{
            //    customer.Id = storedCustomerId;
            //    return CreateInfoValidationResponse(customer, $"Existing customer {customer.FullName} was added.");
            //}
            //else // other existing customer
            //{
            //    //if (storedCustomer.Orders.Count == 1)
            //    //{
            //    //    customerRepository.Delete(storedCustomer);
            //    //}

            //    customer.Id = samePhoneCustomer.Id;
            //    return CreateInfoValidationResponse(customer, $"Other existing customer {samePhoneCustomer.FullName} was added.");
            //}
        }
        
        private static bool IsCustomerNamesNotEqual(Customer customer1, Customer customer2) =>
            customer1.FirstName != customer2.FirstName || customer1.LastName != customer2.LastName;
    }
}