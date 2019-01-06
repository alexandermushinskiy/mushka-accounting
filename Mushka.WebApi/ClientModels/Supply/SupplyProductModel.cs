namespace Mushka.WebApi.ClientModels.Supply
{
    public class SupplyProductModel
    {
        public ProductModel Product { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal CostPrice { get; set; }
    }
}