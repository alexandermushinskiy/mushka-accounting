using System;
using System.Linq;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier;

namespace Mushka.WebApi.Resolvers
{
    public class SupplierRequestConverter : ITypeConverter<SupplierRequestModel, Supplier>
    {
        private readonly IGuidProvider guidProvider;

        public SupplierRequestConverter(IGuidProvider guidProvider)
        {
            this.guidProvider = guidProvider;
        }

        public Supplier Convert(SupplierRequestModel source, Supplier destination, ResolutionContext context)
        {
            var supplierId = guidProvider.NewGuid();

            return new Supplier
            {
                Id = supplierId,
                Name = source.Name,
                Address = source.Address,
                Email = source.Email,
                WebSite = source.WebSite,
                Notes = source.Notes,
                Service = source.Service,
                ContactPersons = source.ContactPersons.Select(cp => CreateContactPerson(supplierId, cp)).ToList()
            };
        }

        private ContactPerson CreateContactPerson(Guid supplierId, ContactPersonRequestModel contactPerson) =>
            new ContactPerson
            {
                SupplierId = supplierId,
                Id = contactPerson.Id ?? guidProvider.NewGuid(),
                Name = contactPerson.Name,
                Email = contactPerson.Email,
                Phones = contactPerson.Phones
            };
    }
}