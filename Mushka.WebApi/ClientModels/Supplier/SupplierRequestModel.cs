using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Supplier
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

        [Required]
        public IEnumerable<ContactPersonRequestModel> ContactPersons { get; set; }

        public IEnumerable<PaymentCardRequestModel> PaymentCards { get; set; }
    }
}