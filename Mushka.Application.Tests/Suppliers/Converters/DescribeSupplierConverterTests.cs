using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Suppliers.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Suppliers.Converters;

public class DescribeSupplierConverterTests
{
    private readonly DescribeSupplierConverter _converter = new DescribeSupplierConverter();

    [Fact]
    public void ToReadModel_MapsSupplierAndChildren()
    {
        var supplierId = Guid.NewGuid();
        var contactPersonId = Guid.NewGuid();
        var paymentCardId = Guid.NewGuid();
        var supplier = new Supplier
        {
            Id = supplierId,
            Name = "Acme",
            Address = "Street",
            Email = "acme@example.com",
            WebSite = "https://acme.example",
            Notes = "note",
            Service = "socks",
            Supplies = new List<Supply> { new Supply(), new Supply() },
            ContactPersons = new List<ContactPerson>
            {
                new ContactPerson { Id = contactPersonId, Name = "Ann", Email = "ann@example.com", Phones = "123" }
            },
            PaymentCards = new List<PaymentCard>
            {
                new PaymentCard { Id = paymentCardId, Number = "1111", Owner = "Ann" }
            }
        };

        var readModel = _converter.ToReadModel(supplier);

        readModel.Supplier.Id.Should().Be(supplierId);
        readModel.Supplier.Name.Should().Be("Acme");
        readModel.Supplier.SuppliesCount.Should().Be(2);
        readModel.ContactPersons.Should().ContainSingle();
        readModel.ContactPersons.Should().ContainSingle(person => person.Id == contactPersonId && person.Name == "Ann");
        readModel.PaymentCards.Should().ContainSingle(card => card.Id == paymentCardId && card.Number == "1111");
    }
}
