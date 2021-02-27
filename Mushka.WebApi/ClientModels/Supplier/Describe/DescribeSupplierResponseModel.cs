using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supplier.Describe
{
    public class DescribeSupplierResponseModel
    {
        public DescribeSupplierModel Supplier { get; set; }

        public IEnumerable<DescribeSupplierContactPersonModel> ContactPersons { get; set; }

        public IEnumerable<DescribeSupplierPaymentCardModel> PaymentCards { get; set; }
    }
}