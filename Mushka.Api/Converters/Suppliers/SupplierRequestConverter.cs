using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Api.ClientModels.Supplier;
using Mushka.Application.Suppliers.Commands;

namespace Mushka.Api.Converters.Suppliers
{
    public sealed class SupplierRequestConverter : ISupplierRequestConverter
    {
        public CreateSupplierCommand ToCreateCommand(CreateSupplierRequestModel request) =>
            new CreateSupplierCommand(
                ToSupplier(request.Supplier),
                ToContactPersons(request.ContactPersons),
                ToPaymentCards(request.PaymentCards));

        public UpdateSupplierCommand ToUpdateCommand(Guid id, UpdateSupplierRequestModel request) =>
            new UpdateSupplierCommand(
                id,
                ToSupplier(request.Supplier),
                ToContactPersons(request.ContactPersons),
                ToPaymentCards(request.PaymentCards));

        private static SupplierCommandModel ToSupplier(SupplierRequestModel supplier) =>
            new SupplierCommandModel(
                supplier.Name,
                supplier.Address,
                supplier.Email,
                supplier.WebSite,
                supplier.Service,
                supplier.Notes);

        private static IReadOnlyList<ContactPersonCommandModel> ToContactPersons(IEnumerable<ContactPersonRequestModel> contactPersons) =>
            (contactPersons ?? Enumerable.Empty<ContactPersonRequestModel>())
                .Select(contactPerson => new ContactPersonCommandModel(
                    contactPerson.Id,
                    contactPerson.Name,
                    contactPerson.Email,
                    contactPerson.Phones))
                .ToList();

        private static IReadOnlyList<PaymentCardCommandModel> ToPaymentCards(IEnumerable<PaymentCardRequestModel> paymentCards) =>
            (paymentCards ?? Enumerable.Empty<PaymentCardRequestModel>())
                .Select(paymentCard => new PaymentCardCommandModel(
                    paymentCard.Id,
                    paymentCard.Number,
                    paymentCard.Owner))
                .ToList();
    }
}
