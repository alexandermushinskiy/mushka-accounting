﻿using System;

namespace Mushka.WebApi.ClientModels.Delivery
{
    public class DeliveryProductModel
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public Guid SizeId { get; set; }

        public string SizeName { get; set; }

        public int Quantity { get; set; }

        public decimal PriceForItem { get; set; }
    }
}