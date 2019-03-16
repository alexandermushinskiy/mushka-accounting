using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.CorporateOrder;

namespace Mushka.WebApi.Resolvers
{
    public class CorporateOrdersListResponseResolver
        : IValueResolver<ValidationResponse<IEnumerable<CorporateOrder>>, CorporateOrdersListResponseModel, IEnumerable<CorporateOrderListModel>>
    {
        public IEnumerable<CorporateOrderListModel> Resolve(
            ValidationResponse<IEnumerable<CorporateOrder>> source,
            CorporateOrdersListResponseModel destination,
            IEnumerable<CorporateOrderListModel> destMember,
            ResolutionContext context)
        {
            return source.Result?.Select(order => new CorporateOrderListModel
            {
                Id = order.Id,
                CreatedOn = order.CreatedOn,
                Number = order.Number,
                CompanyName = order.CompanyName,
                Address = $"{order.Region}, {order.City}"
            });
        }
    }
}