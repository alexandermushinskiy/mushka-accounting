using System.ComponentModel.DataAnnotations;

namespace Mushka.Accounting.WebApi.ClientModels.Category
{
    public class CategoryRequestModel
    {
        [Required]
        public string Name { get; set; }
    }
}