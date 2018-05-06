using System.ComponentModel.DataAnnotations;

namespace Mushka.Accounting.WebApi.ClientModels
{
    public class PostCategoryRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsSizesRequired { get; set; }

        public string Sizes { get; set; }
    }
}