namespace Mushka.WebApi.ClientModels.Product.GetById
{
    public class ProductResponseModel
    {
        public ProductModel Product { get; set; }

        public CategoryModel Category { get; set; }

        public SizeModel Size { get; set; }
    }
}