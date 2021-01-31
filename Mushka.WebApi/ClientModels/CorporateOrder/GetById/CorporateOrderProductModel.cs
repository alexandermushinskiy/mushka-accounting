namespace Mushka.WebApi.ClientModels.CorporateOrder.GetById
{
    public class CorporateOrderProductModel
    {
        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CostPrice { get; set; }
    }
}