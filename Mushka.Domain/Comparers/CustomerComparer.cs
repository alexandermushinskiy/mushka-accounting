using System.Collections.Generic;
using Mushka.Core;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Comparers
{
    public class CustomerComparer : IEqualityComparer<Customer>
    {
        public bool Equals(Customer x, Customer y)
        {
            return x.FirstName == y.FirstName &&
                x.LastName == y.LastName &&
                x.Phone == y.Phone;
        }

        public int GetHashCode(Customer obj)
        {
            return HashCodeGenerator.GetFromValues(new
            {
                obj.FirstName,
                obj.LastName,
                obj.Phone
            });
        }
    }
}