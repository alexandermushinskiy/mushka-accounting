using System;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Exhibition.Describe
{
    public class DescribeExhibitionModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string City { get; set; }

        public decimal ParticipationCost { get; set; }

        public PaymentMethod ParticipationCostMethod { get; set; }

        public decimal? AccommodationCost { get; set; }

        public PaymentMethod? AccommodationCostMethod { get; set; }

        public decimal? FareCost { get; set; }

        public PaymentMethod? FareCostMethod { get; set; }

        public decimal Profit { get; set; }

        public string Notes { get; set; }
    }
}