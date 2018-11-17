using System;

namespace Mushka.WebApi.ClientModels.Supplier
{
    public class SupplierContactPersonModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string Phones { get; set; }
    }
}