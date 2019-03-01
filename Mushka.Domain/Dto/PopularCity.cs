namespace Mushka.Domain.Dto
{
    public class PopularCity
    {
        public string City { get; }

        public int Quantity { get; }

        public PopularCity(string city, int quantity)
        {
            City = city;
            Quantity = quantity;
        }
    }
}