using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier.Describe;

namespace Mushka.WebApi.Resolvers.Suppliers
{
    public class DescribeSupplierResponseConverter : ITypeConverter<OperationResult<Supplier>, DescribeSupplierResponseModel>
    {
        public DescribeSupplierResponseModel Convert(
            OperationResult<Supplier> source,
            DescribeSupplierResponseModel destination,
            ResolutionContext context)
        {
            return new DescribeSupplierResponseModel
            {
                Supplier = ToSupplierModel(source.Data),
                ContactPersons = source.Data.ContactPersons.Select(ToContactPersonModel),
                PaymentCards = source.Data.PaymentCards.Select(ToCardNumberModel)
            };
        }

        private static DescribeSupplierModel ToSupplierModel(Supplier supplier) =>
            new DescribeSupplierModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Address = supplier.Address,
                Email = supplier.Email,
                WebSite = supplier.WebSite,
                Notes = supplier.Notes,
                Service = supplier.Service,
                SuppliesCount = supplier.Supplies.Count
            };

        private static DescribeSupplierContactPersonModel ToContactPersonModel(ContactPerson contactPerson) =>
            new DescribeSupplierContactPersonModel
            {
                Id = contactPerson.Id,
                Name = contactPerson.Name,
                Email = contactPerson.Email,
                Phones = contactPerson.Phones
            };

        private static DescribeSupplierPaymentCardModel ToCardNumberModel(PaymentCard paymentCard) =>
            new DescribeSupplierPaymentCardModel
            {
                Id = paymentCard.Id,
                Number = paymentCard.Number,
                Owner = paymentCard.Owner
            };
    }
}