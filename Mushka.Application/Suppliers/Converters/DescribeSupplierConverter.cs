using System.Linq;
using Mushka.Application.Suppliers.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Suppliers.Converters
{
    public sealed class DescribeSupplierConverter : IDescribeSupplierConverter
    {
        public DescribeSupplierReadModel ToReadModel(Supplier supplier) =>
            new DescribeSupplierReadModel
            {
                Supplier = ToSupplier(supplier),
                ContactPersons = supplier.ContactPersons?.Select(ToContactPerson) ?? Enumerable.Empty<DescribeSupplierContactPersonReadModel>(),
                PaymentCards = supplier.PaymentCards?.Select(ToPaymentCard) ?? Enumerable.Empty<DescribeSupplierPaymentCardReadModel>()
            };

        private static DescribeSupplierDetailsReadModel ToSupplier(Supplier supplier) =>
            new DescribeSupplierDetailsReadModel
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

        private static DescribeSupplierContactPersonReadModel ToContactPerson(ContactPerson contactPerson) =>
            new DescribeSupplierContactPersonReadModel
            {
                Id = contactPerson.Id,
                Name = contactPerson.Name,
                Email = contactPerson.Email,
                Phones = contactPerson.Phones
            };

        private static DescribeSupplierPaymentCardReadModel ToPaymentCard(PaymentCard paymentCard) =>
            new DescribeSupplierPaymentCardReadModel
            {
                Id = paymentCard.Id,
                Number = paymentCard.Number,
                Owner = paymentCard.Owner
            };
    }
}
