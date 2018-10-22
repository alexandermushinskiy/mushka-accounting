using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mushka.Accounting.WebApi.ClientModels.Product
{
    public class ProductRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
        
        public IEnumerable<Guid> Sizes { get; set; }
    }
}