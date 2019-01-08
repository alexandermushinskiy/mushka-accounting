﻿using System;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrderProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}