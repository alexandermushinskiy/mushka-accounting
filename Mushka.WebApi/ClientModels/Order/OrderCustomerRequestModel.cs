using System;
using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrderCustomerRequestModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Email { get; set; }
    }
}