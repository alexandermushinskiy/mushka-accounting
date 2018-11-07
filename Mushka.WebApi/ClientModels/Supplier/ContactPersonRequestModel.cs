using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Supplier
{
    public class ContactPersonRequestModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Position { get; set; }

        public string City { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phones { get; set; }
    }
}