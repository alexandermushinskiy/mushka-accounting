using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supplier.Search
{
    public class SearchSupplierResponseModel
    {
        public SearchSupplierModel Supplier { get; set; }

        public IEnumerable<SearchSupplierContactPersonModel> ContactPersons { get; set; }

        public IEnumerable<SearchSupplierPaymentCardModel> PaymentCards { get; set; }
    }
}