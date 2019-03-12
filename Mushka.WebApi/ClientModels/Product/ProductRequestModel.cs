using System;
using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Product
{
    public class ProductRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string VendorCode { get; set; }

        public decimal? RecommendedPrice { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public Guid? SizeId { get; set; }

        public bool IsAdditional { get; set; }

        public bool IsArchival { get; set; }
    }
}