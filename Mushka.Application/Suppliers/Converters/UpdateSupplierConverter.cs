using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Suppliers.Commands;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;

namespace Mushka.Application.Suppliers.Converters;

public sealed class UpdateSupplierConverter : IUpdateSupplierConverter
{
    private readonly IGuidProvider _guidProvider;

    public UpdateSupplierConverter(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public Supplier ToSupplier(UpdateSupplierCommand command)
    {
        return new Supplier
        {
            Id = command.Id,
            Name = command.Supplier.Name,
            Address = command.Supplier.Address,
            Email = command.Supplier.Email,
            WebSite = command.Supplier.WebSite,
            Notes = command.Supplier.Notes,
            Service = command.Supplier.Service,
            ContactPersons = ToContactPersons(command.Id, command.ContactPersons),
            PaymentCards = ToPaymentCards(command.Id, command.PaymentCards)
        };
    }

    private List<ContactPerson> ToContactPersons(Guid supplierId, IEnumerable<ContactPersonCommandModel> contactPersons) =>
        (contactPersons ?? Enumerable.Empty<ContactPersonCommandModel>())
            .Select(contactPerson => new ContactPerson
            {
                SupplierId = supplierId,
                Id = contactPerson.Id ?? _guidProvider.NewGuid(),
                Name = contactPerson.Name,
                Email = contactPerson.Email,
                Phones = contactPerson.Phones
            })
            .ToList();

    private List<PaymentCard> ToPaymentCards(Guid supplierId, IEnumerable<PaymentCardCommandModel> paymentCards) =>
        (paymentCards ?? Enumerable.Empty<PaymentCardCommandModel>())
            .Select(paymentCard => new PaymentCard
            {
                SupplierId = supplierId,
                Id = paymentCard.Id ?? _guidProvider.NewGuid(),
                Number = paymentCard.Number,
                Owner = paymentCard.Owner
            })
            .ToList();
}
