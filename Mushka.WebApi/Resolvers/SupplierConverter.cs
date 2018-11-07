using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier;

namespace Mushka.WebApi.Resolvers
{
    public class SupplierConverter : ITypeConverter<Supplier, SupplierModel>
    {
        public SupplierModel Convert(Supplier source, SupplierModel destination, ResolutionContext context) =>
            new SupplierModel
            {
                Id = source.Id,
                Name = source.Name,
                Address = source.Address,
                Email = source.Email,
                Phone = source.Phone,
                WebSite = source.WebSite,
                Notes = source.Notes,
                ContactPersons = source.ContactPersons.Select(CreateSupplierContactPersonModel)
            };

        private static SupplierContactPersonModel CreateSupplierContactPersonModel(ContactPerson contactPerson) =>
            new SupplierContactPersonModel
            {
                FirstName = contactPerson.FirstName,
                LastName = contactPerson.LastName,
                Email = contactPerson.Email,
                Phones = contactPerson.Phones,
                City = contactPerson.City,
                Position = contactPerson.Position
            };
    }
}