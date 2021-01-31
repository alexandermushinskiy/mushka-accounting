using System;

namespace Mushka.WebApi.ClientModels.CorporateOrder.GetAll
{
    public class CorporateOrderSummaryModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string OrderNumber { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }
    }
}