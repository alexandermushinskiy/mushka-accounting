using System;

namespace Mushka.WebApi.ClientModels.Supplier.Search
{
    public class SearchSupplierPaymentCardModel
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string Owner { get; set; }
    }
}