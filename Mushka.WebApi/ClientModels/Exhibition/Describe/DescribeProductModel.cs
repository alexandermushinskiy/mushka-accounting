﻿using System;

namespace Mushka.WebApi.ClientModels.Exhibition.Describe
{
    public class DescribeProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public string SizeName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CostPrice { get; set; }
    }
}