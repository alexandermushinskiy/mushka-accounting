using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Delivery
{
    public class DeliveryResponseModel : ResponseModelBase
    {
        public DeliveryModel Data { get; set; }
    }

    public class DeliveriesResponseModel : ResponseModelBase
    {
        public IEnumerable<DeliveryModel> Data { get; set; }
    }
}