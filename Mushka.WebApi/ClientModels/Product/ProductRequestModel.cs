using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Product
{
    public class ProductRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public IEnumerable<Guid> Sizes { get; set; }
    }
}