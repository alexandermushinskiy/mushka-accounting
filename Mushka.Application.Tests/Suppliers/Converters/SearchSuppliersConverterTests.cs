using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Suppliers.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Suppliers.Converters;

public class SearchSuppliersConverterTests
{
    private readonly SearchSuppliersConverter _converter = new SearchSuppliersConverter();

    [Fact]
    public void ToReadModel_MapsSuppliersAndTotal()
    {
        var supplierId = Guid.NewGuid();
        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                Id = supplierId,
                Name = "Acme",
                Supplies = new List<Supply> { new Supply() },
                ContactPersons = new List<ContactPerson> { new ContactPerson { Name = "Ann" } },
                PaymentCards = null
            }
        };

        var readModel = _converter.ToReadModel(suppliers);

        readModel.Total.Should().Be(1);
        readModel.Items.Should().ContainSingle();
        var item = readModel.Items.Should().ContainSingle().Subject;
        item.Supplier.Id.Should().Be(supplierId);
        item.Supplier.SuppliesCount.Should().Be(1);
        item.ContactPersons.Should().ContainSingle(person => person.Name == "Ann");
        item.PaymentCards.Should().BeEmpty();
    }
}
