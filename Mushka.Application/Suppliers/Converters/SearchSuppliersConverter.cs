using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Suppliers.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.Suppliers.Converters;

public sealed class SearchSuppliersConverter : ISearchSuppliersConverter
{
    public SearchSuppliersReadModel ToReadModel(IEnumerable<Supplier> suppliers)
    {
        var items = suppliers.Select(ToItem).ToList();

        return new SearchSuppliersReadModel
        {
            Total = items.Count,
            Items = items
        };
    }

    private static SearchSupplierReadModel ToItem(Supplier supplier) =>
        new SearchSupplierReadModel
        {
            Supplier = ToSupplier(supplier),
            ContactPersons = supplier.ContactPersons?.Select(ToContactPerson) ?? Enumerable.Empty<SearchSupplierContactPersonReadModel>(),
            PaymentCards = supplier.PaymentCards?.Select(ToPaymentCard) ?? Enumerable.Empty<SearchSupplierPaymentCardReadModel>()
        };

    private static SearchSupplierDetailsReadModel ToSupplier(Supplier supplier) =>
        new SearchSupplierDetailsReadModel
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Address = supplier.Address,
            Email = supplier.Email,
            WebSite = supplier.WebSite,
            Notes = supplier.Notes,
            Service = supplier.Service,
            SuppliesCount = supplier.Supplies?.Count ?? 0
        };

    private static SearchSupplierContactPersonReadModel ToContactPerson(ContactPerson contactPerson) =>
        new SearchSupplierContactPersonReadModel
        {
            Id = contactPerson.Id,
            Name = contactPerson.Name,
            Email = contactPerson.Email,
            Phones = contactPerson.Phones
        };

    private static SearchSupplierPaymentCardReadModel ToPaymentCard(PaymentCard paymentCard) =>
        new SearchSupplierPaymentCardReadModel
        {
            Id = paymentCard.Id,
            Number = paymentCard.Number,
            Owner = paymentCard.Owner
        };
}
