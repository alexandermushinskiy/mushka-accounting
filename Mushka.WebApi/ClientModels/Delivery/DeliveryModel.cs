﻿using System;
using System.Collections.Generic;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Delivery
{
    public class DeliveryModel
    {
        public Guid Id { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public decimal Cost { get; set; }

        public decimal TransferFee { get; set; }

        public decimal BankFee { get; set; }

        public decimal TotalCost { get; set; }

        public string Supplier { get; set; }

        public int ProductsAmount { get; set; }

        public IEnumerable<DeliveryProductModel> Products { get; set; }
    }
}