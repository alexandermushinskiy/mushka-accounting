using System;

namespace Mushka.WebApi.ClientModels.Supplier.Modify
{
    public class PaymentCardRequestModel
    {
        public Guid? Id { get; set; }

        public string Number { get; set; }

        public string Owner { get; set; }
    }
}