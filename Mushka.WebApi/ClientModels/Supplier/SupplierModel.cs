using System;
using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supplier
{
    public class SupplierModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Notes { get; set; }

        public IEnumerable<SupplierContactPersonModel> ContactPersons { get; set; }
    }
}