﻿using System;

namespace Mushka.WebApi.ClientModels.Supplier
{
    public class PaymentCardModel
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string Owner { get; set; }
    }
}