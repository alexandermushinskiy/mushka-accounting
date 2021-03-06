﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class SelectProductsResponseResolver :
        IValueResolver<OperationResult<IEnumerable<Product>>, SelectProductsResponseModel, IEnumerable<SelectProductModel>>
    {
        public IEnumerable<SelectProductModel> Resolve(
            OperationResult<IEnumerable<Product>> source,
            SelectProductsResponseModel destination,
            IEnumerable<SelectProductModel> destMember,
            ResolutionContext context)
        {
            return source.Data?.Select(product => new SelectProductModel
            {
                Id = product.Id,
                Name = product.Name,
                VendorCode = product.VendorCode,
                RecommendedPrice = product.RecommendedPrice,
                Quantity = product.Quantity,
                CategoryName = product.Category.Name,
                SizeName = product.Size?.Name,
                IsArchival = product.IsArchival
            });
        }
    }
}