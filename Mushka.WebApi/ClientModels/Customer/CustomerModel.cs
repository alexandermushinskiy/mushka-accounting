using System;

namespace Mushka.WebApi.ClientModels.Customer
{
    public class CustomerModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string Region { get; set; }
    }
}