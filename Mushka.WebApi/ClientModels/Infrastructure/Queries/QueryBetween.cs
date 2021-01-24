using System;

namespace Mushka.WebApi.ClientModels.Infrastructure.Queries
{
    public class QueryBetween
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}