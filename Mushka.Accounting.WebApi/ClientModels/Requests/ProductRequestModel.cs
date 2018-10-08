using System.ComponentModel.DataAnnotations;

namespace Mushka.Accounting.WebApi.ClientModels.Requests
{
    public class ProductRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public int TotalCount { get; set; }

        public string Sizes { get; set; }
    }
}