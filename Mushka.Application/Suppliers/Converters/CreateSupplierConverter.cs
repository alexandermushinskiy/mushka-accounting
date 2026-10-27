using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Suppliers.Commands;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;

namespace Mushka.Application.Suppliers.Converters;

public sealed class CreateSupplierConverter : ICreateSupplierConverter
{
    private readonly IGuidProvider _guidProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateSupplierConverter(IGuidProvider guidProvider, IDateTimeProvider dateTimeProvider)
    {
        _guidProvider = guidProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public Supplier ToSupplier(CreateSupplierCommand command)
    {
        var supplierId = _guidProvider.NewGuid();

        return new Supplier
        {
            Id = supplierId,
            Name = command.Supplier.Name,
            Address = command.Supplier.Address,
            Email = command.Supplier.Email,
            WebSite = command.Supplier.WebSite,
            Notes = command.Supplier.Notes,
            Service = command.Supplier.Service,
            CreatedOn = _dateTimeProvider.GetNow(),
            ContactPersons = ToContactPersons(supplierId, command.ContactPersons),
            PaymentCards = ToPaymentCards(supplierId, command.PaymentCards)
        };
    }

    private List<ContactPerson> ToContactPersons(Guid supplierId, IEnumerable<ContactPersonData> contactPersons) =>
        (contactPersons ?? Enumerable.Empty<ContactPersonData>())
            .Select(contactPerson => new ContactPerson
            {
                SupplierId = supplierId,
                Id = contactPerson.Id ?? _guidProvider.NewGuid(),
                Name = contactPerson.Name,
                Email = contactPerson.Email,
                Phones = contactPerson.Phones
            })
            .ToList();

    private List<PaymentCard> ToPaymentCards(Guid supplierId, IEnumerable<PaymentCardData> paymentCards) =>
        (paymentCards ?? Enumerable.Empty<PaymentCardData>())
            .Select(paymentCard => new PaymentCard
            {
                SupplierId = supplierId,
                Id = paymentCard.Id ?? _guidProvider.NewGuid(),
                Number = paymentCard.Number,
                Owner = paymentCard.Owner
            })
            .ToList();
}
