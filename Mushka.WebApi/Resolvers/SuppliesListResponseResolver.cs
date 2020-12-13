using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.Dto;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Supply;

namespace Mushka.WebApi.Resolvers
{
    public class SuppliesListResponseResolver : 
        IValueResolver<ValidationResponse<ItemsWithTotalCount<Supply>>, DataWithTotalCountResponseModel<SupplyListModel>, ItemsWithCountResponseModel<SupplyListModel>>,
        IValueResolver<OperationResult<ItemsWithTotalCount<Supply>>, DataWithTotalCountResponseModel<SupplyListModel>, ItemsWithCountResponseModel<SupplyListModel>>
    {
        public ItemsWithCountResponseModel<SupplyListModel> Resolve(
            ValidationResponse<ItemsWithTotalCount<Supply>> source,
            DataWithTotalCountResponseModel<SupplyListModel> destination,
            ItemsWithCountResponseModel<SupplyListModel> destMember, 
            ResolutionContext context)
        {
            return new ItemsWithCountResponseModel<SupplyListModel>
            {
                TotalCount = source.Result.TotalCount,
                Items = source.Result.Data.Select(supply => new SupplyListModel
                {
                    Id = supply.Id,
                    Description = supply.Description,
                    SupplierName = supply.Supplier?.Name,
                    ReceivedDate = supply.ReceivedDate,
                    Cost = supply.Cost,
                    TotalCost = supply.TotalCost,
                    ProductsAmount = supply.Products.Sum(prod => prod.Quantity),
                })
            };
        }

        public ItemsWithCountResponseModel<SupplyListModel> Resolve(
            OperationResult<ItemsWithTotalCount<Supply>> source,
            DataWithTotalCountResponseModel<SupplyListModel> destination,
            ItemsWithCountResponseModel<SupplyListModel> destMember,
            ResolutionContext context)
        {
            return new ItemsWithCountResponseModel<SupplyListModel>
            {
                TotalCount = source.Data.TotalCount,
                Items = source.Data.Data.Select(supply => new SupplyListModel
                {
                    Id = supply.Id,
                    Description = supply.Description,
                    SupplierName = supply.Supplier?.Name,
                    ReceivedDate = supply.ReceivedDate,
                    Cost = supply.Cost,
                    TotalCost = supply.TotalCost,
                    ProductsAmount = supply.Products.Sum(prod => prod.Quantity),
                })
            };
        }
    }
}