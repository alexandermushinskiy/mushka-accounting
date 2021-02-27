using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Supplier.Modify
{
    public class SupplierRequestModel
    {
        [Required]
        public string Name { get; set; }

        public string Address { get; set; }
        
        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Service { get; set; }

        public string Notes { get; set; }
    }
}