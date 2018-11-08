using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Supplier
{
    public class ContactPersonRequestModel
    {
        [Required]
        public string Name { get; set; }
        
        public string Position { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string Phones { get; set; }
    }
}