using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mushka.Api.ClientModels.Supplier;

public class CreateSupplierRequestModel
{
    [Required]
    public SupplierRequestModel Supplier { get; set; }

    [Required]
    public IEnumerable<ContactPersonRequestModel> ContactPersons { get; set; }

    public IEnumerable<PaymentCardRequestModel> PaymentCards { get; set; }
}
