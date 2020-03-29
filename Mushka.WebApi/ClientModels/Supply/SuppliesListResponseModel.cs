using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class SuppliesListResponseModel : ResponseModelBase
    {
        public IEnumerable<SupplyListModel> Data { get; set; }
    }

    //public class SuppliesWithCountResponseModel : DataWithTotalCountResponseModel<SupplyListModel>
    //{
    //}

    public class DataWithTotalCountResponseModel<TData> : ResponseModelBase
    {
        public ItemsWithCountResponseModel<TData> Data { get; set; }
    }

    public class ItemsWithCountResponseModel<TData>
    {
        public IEnumerable<TData> Items { get; set; }
        public int TotalCount { get; set; }
    }
}