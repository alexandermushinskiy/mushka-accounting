using System.Collections.Generic;
using System.Linq;

namespace Mushka.Service.Extensibility.Dto
{
    public class ItemsWithTotalCount<TDto>
    {
        public IEnumerable<TDto> Data { get; }
        public int TotalCount { get; }

        public ItemsWithTotalCount(IEnumerable<TDto> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public static ItemsWithTotalCount<TDto> Empty() =>
            new ItemsWithTotalCount<TDto>(Enumerable.Empty<TDto>(), 0);
    }
}