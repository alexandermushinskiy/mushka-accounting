using AutoMapper;

namespace Mushka.WebApi.ClientModels.CorporateOrder.GetAll
{
    public class CorporateOrderSummaryConverter : ITypeConverter<Domain.Entities.CorporateOrder, CorporateOrderSummaryModel>
    {
        public CorporateOrderSummaryModel Convert(
            Domain.Entities.CorporateOrder source,
            CorporateOrderSummaryModel destination,
            ResolutionContext context)
        {
            return new CorporateOrderSummaryModel
            {
                Id = source.Id,
                CreatedOn = source.CreatedOn,
                OrderNumber = source.Number,
                CompanyName = source.CompanyName,
                Address = $"{source.Region}, {source.City}"
            };
        }
    }
}