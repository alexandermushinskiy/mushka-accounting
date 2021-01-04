namespace Mushka.Domain.Models
{
    public class ItemsList<TData>
    {
        public TData[] Items { get; }
        public int Total { get; }

        public ItemsList(TData[] items, int total)
        {
            Items = items;
            Total = total;
        }
    }
}