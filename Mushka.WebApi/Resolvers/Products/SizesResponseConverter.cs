using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers.Products
{
    public class SizesResponseConverter : ITypeConverter<OperationResult<IEnumerable<Size>>, SizesResponseModel>
    {
        public SizesResponseModel Convert(OperationResult<IEnumerable<Size>> source, SizesResponseModel destination, ResolutionContext context)
        {
            return new SizesResponseModel
            {
                Items = source.Data.Select(sz => new SizeModel { Id = sz.Id, Name = sz.Name })
            };
        }
    }
}