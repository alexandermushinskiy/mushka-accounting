namespace Mushka.WebApi.ClientModels.Supply
{
    public class SupplyProductModel
    {
        public ProductModel Product { get; set; }

        public decimal CostForItem { get; set; }

        public int Quantity { get; set; }
    }
}