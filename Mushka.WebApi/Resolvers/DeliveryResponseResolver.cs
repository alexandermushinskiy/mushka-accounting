using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Delivery;

namespace Mushka.WebApi.Resolvers
{
    public class DeliveryResponseResolver :
        IValueResolver<ValidationResponse<Delivery>, DeliveryResponseModel, DeliveryModel>,
        IValueResolver<ValidationResponse<IEnumerable<Delivery>>, DeliveriesResponseModel, IEnumerable<DeliveryModel>>
    {
        public DeliveryModel Resolve(
            ValidationResponse<Delivery> source,
            DeliveryResponseModel destination,
            DeliveryModel destMember,
            ResolutionContext context) => source.Result == null ? null : Mapper.Map<Delivery, DeliveryModel>(source.Result);

        public IEnumerable<DeliveryModel> Resolve(
            ValidationResponse<IEnumerable<Delivery>> source,
            DeliveriesResponseModel destination,
            IEnumerable<DeliveryModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<Delivery, DeliveryModel>);
    }
}