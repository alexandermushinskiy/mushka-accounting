using System;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Entities;
using Mushka.Domain.Models;
using Mushka.Domain.Extensibility.Specifications;

namespace Mushka.Infrastructure.DataAccess.Specifications
{
    public class SearchOrdersSpecification : BaseSpecification<Order>
    {
        public SearchOrdersSpecification(SearchOrdersFilter searchOrdersFilter)
        {
            Criteria = order =>
                searchOrdersFilter.OrderDate.From == null || (order.OrderDate >= searchOrdersFilter.OrderDate.From) &&
                searchOrdersFilter.OrderDate.To == null || (order.OrderDate <= searchOrdersFilter.OrderDate.To) &&
                String.IsNullOrEmpty(searchOrdersFilter.SearchKey) ||
                                    EF.Functions.Like(order.Customer.FullName, $"%{searchOrdersFilter.SearchKey}%") ||
                                    EF.Functions.Like(order.Number, $"%{searchOrdersFilter.SearchKey}%");

            AddInclude(order => order.Customer);

            AddOrderBy(
                searchOrdersFilter.SortKey,
                searchOrdersFilter.IsAsc);

            ApplyPaging(
                searchOrdersFilter.CurrentPage * searchOrdersFilter.PageSize,
                searchOrdersFilter.PageSize);
        }
    }
}
