using System.Collections.Generic;
using System.Linq;

namespace Mushka.Core
{
    public static class HashCodeGenerator
    {
        public static int GetFromValues(params object[] values) => GetFromValues(values.ToList());

        public static int GetFromValues(IEnumerable<object> values) => GetFromHashCodes(values.Where(value => value != null).Select(value => value.GetHashCode()).ToArray());

        public static int GetFromHashCodes(params int[] hashCodes) => GetFromHashCodes(hashCodes.ToList());

        public static int GetFromHashCodes(IEnumerable<int> hashCodes) => hashCodes.Aggregate(17, (current, value) => (current * 59) + value);
    }
}