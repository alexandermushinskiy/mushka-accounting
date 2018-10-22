﻿using System.Linq;
using AutoMapper;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.WebApi.ClientModels.Product;

namespace Mushka.Accounting.WebApi.Resolvers
{
    public class ProductConverter : ITypeConverter<Product, ProductModel>
    {
        public ProductModel Convert(Product source, ProductModel destination, ResolutionContext context) =>
            new ProductModel
                {
                    Id = source.Id,
                    Name = source.Name,
                    Code = source.Code,
                    CreatedOn = source.CreatedOn,
                    Sizes = source.Sizes.Select(s => s.SizeId).ToArray()
                };
    }
}