using System;

namespace Mushka.WebApi.ClientModels.Exhibition
{
    public class ExhibitionsListModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string City { get; set; }

        public decimal ParticipationCost { get; set; }

        public int ProductsCount { get; set; }
    }
}