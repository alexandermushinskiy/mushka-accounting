using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Mushka.Application.Suppliers.Commands;
using Mushka.Application.Suppliers.Converters;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Suppliers.Converters
{
    public class UpdateSupplierConverterTests
    {
        private readonly Guid _generatedId = Guid.Parse("00000000-0000-0000-0000-000000000009");
        private readonly UpdateSupplierConverter _converter;

        public UpdateSupplierConverterTests()
        {
            var guidProvider = new Mock<IGuidProvider>();
            guidProvider.Setup(provider => provider.NewGuid()).Returns(_generatedId);
            _converter = new UpdateSupplierConverter(guidProvider.Object);
        }

        [Fact]
        public void ToSupplier_UsesCommandId_AndAssignsMissingChildIds()
        {
            var supplierId = Guid.NewGuid();
            var contactPersonId = Guid.NewGuid();
            var command = new UpdateSupplierCommand(
                supplierId,
                new SupplierCommandModel("Acme", "Street", "acme@example.com", "https://acme.example", "socks", "note"),
                new List<ContactPersonCommandModel> { new ContactPersonCommandModel(contactPersonId, "Ann", "ann@example.com", "123") },
                new List<PaymentCardCommandModel> { new PaymentCardCommandModel(null, "1111", "Ann") });

            var supplier = _converter.ToSupplier(command);

            supplier.Id.Should().Be(supplierId);
            supplier.Name.Should().Be("Acme");
            supplier.CreatedOn.Should().Be(default(DateTime));
            supplier.ContactPersons.Should().ContainSingle()
                .Which.Id.Should().Be(contactPersonId);
            supplier.PaymentCards.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new PaymentCard
                {
                    Id = _generatedId,
                    SupplierId = supplierId,
                    Number = "1111",
                    Owner = "Ann"
                });
        }

        [Fact]
        public void ToSupplier_WhenCollectionsAreNull_ReturnsEmptyCollections()
        {
            var command = new UpdateSupplierCommand(
                Guid.NewGuid(),
                new SupplierCommandModel("Acme", null, null, null, null, null),
                null,
                null);

            var supplier = _converter.ToSupplier(command);

            supplier.ContactPersons.Should().BeEmpty();
            supplier.PaymentCards.Should().BeEmpty();
        }
    }
}
