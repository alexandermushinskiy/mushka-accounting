﻿using System.Collections.Generic;
using Mushka.Domain.Dto;

namespace Mushka.WebApi.ClientModels.Analytics
{
    public class PopluarProductsResponseModel : ResponseModelBase
    {
        public IEnumerable<PopularProduct> Data { get; set; }
    }
}