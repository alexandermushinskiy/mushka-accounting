using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Mushka.Application.Suppliers.Commands;
using Mushka.Application.Suppliers.Converters;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Suppliers.Converters;

public class CreateSupplierConverterTests
{
    private readonly Guid _supplierId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private readonly Guid _contactPersonId = Guid.Parse("00000000-0000-0000-0000-000000000002");
    private readonly Guid _paymentCardId = Guid.Parse("00000000-0000-0000-0000-000000000003");
    private readonly DateTime _createdOn = new DateTime(2024, 5, 6, 7, 8, 9);
    private readonly CreateSupplierConverter _converter;

    public CreateSupplierConverterTests()
    {
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.SetupSequence(provider => provider.NewGuid())
            .Returns(_supplierId)
            .Returns(_contactPersonId)
            .Returns(_paymentCardId);
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        dateTimeProvider.Setup(provider => provider.GetNow()).Returns(_createdOn);
        _converter = new CreateSupplierConverter(guidProvider.Object, dateTimeProvider.Object);
    }

    [Fact]
    public void ToSupplier_MapsCommand_AndAssignsGeneratedIds()
    {
        var contactPersonId = Guid.NewGuid();
        var command = new CreateSupplierCommand(
            new SupplierData("Acme", "Street", "acme@example.com", "https://acme.example", "socks", "note"),
            new List<ContactPersonData>
            {
                new ContactPersonData(contactPersonId, "Ann", "ann@example.com", "123"),
                new ContactPersonData(null, "Bob", null, null)
            },
            new List<PaymentCardData>
            {
                new PaymentCardData(null, "1111", "Ann")
            });

        var supplier = _converter.ToSupplier(command);

        supplier.Id.Should().Be(_supplierId);
        supplier.Name.Should().Be("Acme");
        supplier.Address.Should().Be("Street");
        supplier.Email.Should().Be("acme@example.com");
        supplier.WebSite.Should().Be("https://acme.example");
        supplier.Service.Should().Be("socks");
        supplier.Notes.Should().Be("note");
        supplier.CreatedOn.Should().Be(_createdOn);
        supplier.ContactPersons.Should().BeEquivalentTo(new[]
        {
            new ContactPerson { Id = contactPersonId, SupplierId = _supplierId, Name = "Ann", Email = "ann@example.com", Phones = "123" },
            new ContactPerson { Id = _contactPersonId, SupplierId = _supplierId, Name = "Bob" }
        });
        supplier.PaymentCards.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new PaymentCard { Id = _paymentCardId, SupplierId = _supplierId, Number = "1111", Owner = "Ann" });
    }
}
