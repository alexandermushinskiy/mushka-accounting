using System;

namespace Mushka.WebApi.ClientModels.Supplier.Describe
{
    public class DescribeSupplierModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Notes { get; set; }

        public string Service { get; set; }

        public string PaymentMethods { get; set; }

        public int SuppliesCount { get; set; }
    }
}