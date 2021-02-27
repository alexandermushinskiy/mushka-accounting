using System;
using System.Linq;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier.Modify;
using Mushka.WebApi.Extensions;

namespace Mushka.WebApi.Resolvers.Suppliers
{
    public class CreateSupplierRequestConverter : ITypeConverter<CreateSupplierRequestModel, Supplier>
    {
        private readonly IGuidProvider guidProvider;
        private readonly IDateTimeProvider dateTimeProvider;

        public CreateSupplierRequestConverter(
            IGuidProvider guidProvider,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidProvider = guidProvider;
            this.dateTimeProvider = dateTimeProvider;
        }

        public Supplier Convert(CreateSupplierRequestModel source, Supplier destination, ResolutionContext context)
        {
            var supplierId = context.GetId() ?? guidProvider.NewGuid();

            return new Supplier
            {
                Id = supplierId,
                Name = source.Supplier.Name,
                Address = source.Supplier.Address,
                Email = source.Supplier.Email,
                WebSite = source.Supplier.WebSite,
                Notes = source.Supplier.Notes,
                Service = source.Supplier.Service,
                CreatedOn = dateTimeProvider.GetNow(),
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