using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Category
{
    public class CategoryRequestModel
    {
        [Required]
        public string Name { get; set; }

        public bool IsSizeRequired { get; set; }

        public bool IsAdditional { get; set; }
    }
}