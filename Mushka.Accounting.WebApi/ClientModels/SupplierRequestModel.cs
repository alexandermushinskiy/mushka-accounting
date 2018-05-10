using System.ComponentModel.DataAnnotations;

namespace Mushka.Accounting.WebApi.ClientModels
{
    public class SupplierRequestModel
    {
        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string ContactPerson { get; set; }

        public string PaymentConditions { get; set; }

        public string Services { get; set; }

        public string Comments { get; set; }
    }
}