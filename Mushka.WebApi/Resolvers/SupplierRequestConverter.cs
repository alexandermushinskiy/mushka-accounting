using System;
using System.Linq;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier;
using Mushka.WebApi.Extensions;

namespace Mushka.WebApi.Resolvers
{
    public class SupplierRequestConverter : ITypeConverter<SupplierRequestModel, Supplier>
    {
        private readonly IGuidProvider guidProvider;
        private readonly IDateTimeProvider dateTimeProvider;

        public SupplierRequestConverter(
            IGuidProvider guidProvider,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidProvider = guidProvider;
            this.dateTimeProvider = dateTimeProvider;
        }

        public Supplier Convert(SupplierRequestModel source, Supplier destination, ResolutionContext context)
        {
            var supplierId = context.GetId() ?? guidProvider.NewGuid();

            return new Supplier
            {
                Id = supplierId,
                Name = source.Name,
                Address = source.Address,
                Email = source.Email,
                WebSite = source.WebSite,
                Notes = source.Notes,
                Service = source.Service,
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