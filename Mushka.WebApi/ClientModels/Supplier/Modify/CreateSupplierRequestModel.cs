using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mushka.WebApi.ClientModels.Supplier.Modify
{
    public class CreateSupplierRequestModel
    {
        [Required]
        public SupplierRequestModel Supplier { get; set; }

        [Required]
        public IEnumerable<ContactPersonRequestModel> ContactPersons { get; set; }

        public IEnumerable<PaymentCardRequestModel> PaymentCards { get; set; }
    }
}