using System;

namespace Mushka.WebApi.ClientModels.CorporateOrder
{
    public class CorporateOrderListModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Number { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }
    }
}