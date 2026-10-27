using System;
using System.Linq;
using FluentAssertions;
using Mushka.Api.ClientModels.Supplier;
using Mushka.Api.Converters.Suppliers;
using Xunit;

namespace Mushka.Api.Tests.Converters.Suppliers;

public class SupplierRequestConverterTests
{
    private readonly SupplierRequestConverter _converter = new SupplierRequestConverter();

    [Fact]
    public void ToCreateCommand_MapsRequest()
    {
        var contactPersonId = Guid.NewGuid();
        var paymentCardId = Guid.NewGuid();
        var request = Request(contactPersonId, paymentCardId);

        var command = _converter.ToCreateCommand(request);

        command.Supplier.Name.Should().Be("Acme");
        command.Supplier.Address.Should().Be("Street");
        command.Supplier.Email.Should().Be("acme@example.com");
        command.Supplier.WebSite.Should().Be("https://acme.example");
        command.Supplier.Service.Should().Be("socks");
        command.Supplier.Notes.Should().Be("note");
        command.ContactPersons.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new { Id = (Guid?)contactPersonId, Name = "Ann", Email = "ann@example.com", Phones = "123" });
        command.PaymentCards.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new { Id = (Guid?)paymentCardId, Number = "1111", Owner = "Ann" });
    }

    [Fact]
    public void ToUpdateCommand_UsesRouteId_AndMapsRequest()
    {
        var id = Guid.NewGuid();
        var request = new UpdateSupplierRequestModel
        {
            Supplier = new SupplierRequestModel { Name = "Acme" },
            ContactPersons = Enumerable.Empty<ContactPersonRequestModel>()
        };

        var command = _converter.ToUpdateCommand(id, request);

        command.Id.Should().Be(id);
        command.Supplier.Name.Should().Be("Acme");
    }

    [Fact]
    public void ToCreateCommand_WhenCollectionsAreNull_ReturnsEmptyCollections()
    {
        var request = new CreateSupplierRequestModel
        {
            Supplier = new SupplierRequestModel { Name = "Acme" }
        };

        var command = _converter.ToCreateCommand(request);

        command.ContactPersons.Should().BeEmpty();
        command.PaymentCards.Should().BeEmpty();
    }

    private static CreateSupplierRequestModel Request(Guid? contactPersonId, Guid? paymentCardId) =>
        new CreateSupplierRequestModel
        {
            Supplier = new SupplierRequestModel
            {
                Name = "Acme",
                Address = "Street",
                Email = "acme@example.com",
                WebSite = "https://acme.example",
                Service = "socks",
                Notes = "note"
            },
            ContactPersons = new[]
            {
                new ContactPersonRequestModel
                {
                    Id = contactPersonId,
                    Name = "Ann",
                    Email = "ann@example.com",
                    Phones = "123"
                }
            },
            PaymentCards = new[]
            {
                new PaymentCardRequestModel
                {
                    Id = paymentCardId,
                    Number = "1111",
                    Owner = "Ann"
                }
            }
        };
}
