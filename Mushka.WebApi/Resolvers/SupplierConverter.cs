using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier;

namespace Mushka.WebApi.Resolvers
{
    public class SupplierConverter : ITypeConverter<Supplier, SupplierModel>
    {
        public SupplierModel Convert(Supplier source, SupplierModel destination, ResolutionContext context) =>
            new SupplierModel
            {
                Id = source.Id,
                Name = source.Name,
                Address = source.Address,
                Email = source.Email,
                WebSite = source.WebSite,
                Notes = source.Notes,
                Service = source.Service,
                SuppliesCount = source.Supplies.Count,
                ContactPersons = source.ContactPersons.Select(CreateSupplierContactPersonModel),
                PaymentCards = source.PaymentCards.Select(CreateCardNumberModel)
            };

        private static SupplierContactPersonModel CreateSupplierContactPersonModel(ContactPerson contactPerson) =>
            new SupplierContactPersonModel
            {
                Id = contactPerson.Id,
                Name = contactPerson.Name,
                Email = contactPerson.Email,
                Phones = contactPerson.Phones
            };

        private static PaymentCardModel CreateCardNumberModel(PaymentCard paymentCard) =>
            new PaymentCardModel
            {
                Id = paymentCard.Id,
                Number = paymentCard.Number,
                Owner = paymentCard.Owner
            };
    }
}