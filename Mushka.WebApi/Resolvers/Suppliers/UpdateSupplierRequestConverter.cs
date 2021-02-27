using System;
using System.Linq;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier.Modify;
using Mushka.WebApi.Extensions;

namespace Mushka.WebApi.Resolvers.Suppliers
{
    public class UpdateSupplierRequestConverter : ITypeConverter<UpdateSupplierRequestModel, Supplier>
    {
        private readonly IGuidProvider guidProvider;

        public UpdateSupplierRequestConverter(IGuidProvider guidProvider)
        {
            this.guidProvider = guidProvider;
        }

        public Supplier Convert(UpdateSupplierRequestModel source, Supplier destination, ResolutionContext context)
        {
            var supplierId = context.GetId().Value;

            return new Supplier
            {
                Id = supplierId,
                Name = source.Supplier.Name,
                Address = source.Supplier.Address,
                Email = source.Supplier.Email,
                WebSite = source.Supplier.WebSite,
                Notes = source.Supplier.Notes,
                Service = source.Supplier.Service,
                ContactPersons = source.ContactPersons.Select(cp => CreateContactPerson(supplierId, cp)).ToList(),
                PaymentCards = source.PaymentCards.Select(pc => CreatePaymentCard(supplierId, pc)).ToList()
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

        private PaymentCard CreatePaymentCard(Guid supplierId, PaymentCardRequestModel requestModel) =>
            new PaymentCard
            {
                SupplierId = supplierId,
                Id = requestModel.Id ?? guidProvider.NewGuid(),
                Number = requestModel.Number,
                Owner = requestModel.Owner
            };
    }
}