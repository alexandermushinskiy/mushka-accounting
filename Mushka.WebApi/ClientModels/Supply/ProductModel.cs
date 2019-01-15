﻿using System;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class ProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public string SizeName { get; set; }
    }
}