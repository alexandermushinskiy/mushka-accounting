using System;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class SupplyListModel
    {
        public Guid Id { get; set; }
        
        public DateTime ReceivedDate { get; set; }

        public string Description { get; set; }

        public string SupplierName { get; set; }

        public int ProductsAmount { get; set; }

        public decimal Cost { get; set; }

        public decimal TotalCost { get; set; }
    }
}