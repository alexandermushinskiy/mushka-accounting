using System;

namespace Mushka.WebApi.ClientModels.Supplier.Describe
{
    public class DescribeSupplierPaymentCardModel
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string Owner { get; set; }
    }
}