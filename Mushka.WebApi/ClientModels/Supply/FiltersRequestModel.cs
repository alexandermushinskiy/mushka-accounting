using System;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class SuppliesFiltersRequestModel
    {
        public string SearchKey { get; set; }
        public Guid? ProductId { get; set; }
    }
}