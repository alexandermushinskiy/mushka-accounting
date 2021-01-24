namespace Mushka.WebApi.ClientModels.Infrastructure.Queries
{
    public class QueryLike
    {
        public string Like { get; set; }

        public static implicit operator string(QueryLike queryLike)
        {
            return queryLike?.Like;
        }
    }
}