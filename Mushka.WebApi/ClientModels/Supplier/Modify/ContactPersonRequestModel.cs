using System;
using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Supplier.Modify
{
    public class ContactPersonRequestModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string Email { get; set; }

        public string Phones { get; set; }
    }
}