using System;

namespace Mushka.WebApi.ClientModels.Category.Search
{
    public class CategorySummaryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsSizeRequired { get; set; }

        public bool IsAdditional { get; set; }
    }
}