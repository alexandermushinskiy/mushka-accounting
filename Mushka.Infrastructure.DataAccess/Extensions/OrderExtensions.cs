using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Extensions
{
    public static class OrderExtensions
    {
        public static string CustomerFullNameLower(this Order order)
        {
            return string.Concat(order.Customer.FirstName, " ", order.Customer.LastName).ToLower();
        }
    }
}
