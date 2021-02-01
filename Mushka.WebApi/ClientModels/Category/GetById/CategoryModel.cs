using System;

namespace Mushka.WebApi.ClientModels.Category.GetById
{
    public class CategoryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsSizeRequired { get; set; }

        public bool IsAdditional { get; set; }
    }
}