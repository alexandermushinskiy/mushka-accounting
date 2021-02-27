using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier.Search;

namespace Mushka.WebApi.Resolvers.Suppliers
{
    public class SearchSuppliersResponseConverter : ITypeConverter<OperationResult<IEnumerable<Supplier>>, SearchSuppliersResponseModel>
    {
        public SearchSuppliersResponseModel Convert(
            OperationResult<IEnumerable<Supplier>> source,
            SearchSuppliersResponseModel destination,
            ResolutionContext context)
        {
            return new SearchSuppliersResponseModel
            {
                Total = source.Data?.Count() ?? 0,
                Items = source.Data?.Select(Convert) ?? Enumerable.Empty<SearchSupplierResponseModel>()
            };
        }

        private static SearchSupplierResponseModel Convert(Supplier source) =>
            new SearchSupplierResponseModel
            {
                Supplier = ToSupplierModel(source),
                ContactPersons = source.ContactPersons.Select(ToContactPersonModel),
                PaymentCards = source.PaymentCards.Select(ToCardNumberModel)
            };

        private static SearchSupplierModel ToSupplierModel(Supplier supplier) =>
            new SearchSupplierModel
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

        private static SearchSupplierContactPersonModel ToContactPersonModel(ContactPerson contactPerson) =>
            new SearchSupplierContactPersonModel
            {
                Id = contactPerson.Id,
                Name = contactPerson.Name,
                Email = contactPerson.Email,
                Phones = contactPerson.Phones
            };

        private static SearchSupplierPaymentCardModel ToCardNumberModel(PaymentCard paymentCard) =>
            new SearchSupplierPaymentCardModel
            {
                Id = paymentCard.Id,
                Number = paymentCard.Number,
                Owner = paymentCard.Owner
            };
    }
}