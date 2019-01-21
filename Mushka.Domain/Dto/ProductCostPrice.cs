namespace Mushka.Domain.Dto
{
    public class ProductCostPrice
    {
        public decimal CostPrice { get; }

        public ProductCostPrice(decimal costPrice)
        {
            CostPrice = costPrice;
        }
    }
}