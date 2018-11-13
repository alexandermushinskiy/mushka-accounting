using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Supplier
{
    public class SupplierRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
        
        [Required]
        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Service { get; set; }

        public string Notes { get; set; }

        [Required]
        public IEnumerable<ContactPersonRequestModel> ContactPersons { get; set; }
    }
}