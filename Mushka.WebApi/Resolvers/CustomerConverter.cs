using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Customer;

namespace Mushka.WebApi.Resolvers
{
    public class CustomerConverter : ITypeConverter<Customer, CustomerModel>
    {
        public CustomerModel Convert(Customer source, CustomerModel destination, ResolutionContext context) =>
            new CustomerModel
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                City = source.City,
                Region = source.Region,
                Phone = source.Phone,
                Email = source.Email
            };
    }
}