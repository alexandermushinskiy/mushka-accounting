namespace Mushka.Domain.Dto
{
    public class PopularProduct
    {
        public string Name { get; }

        public string SizeName { get; }

        public string VendorCode { get; }

        public int Quantity { get; }

        public PopularProduct(string name, string sizeName, string vendorCode, int quantity)
        {
            Name = name;
            SizeName = sizeName;
            VendorCode = vendorCode;
            Quantity = quantity;
        }
    }
}