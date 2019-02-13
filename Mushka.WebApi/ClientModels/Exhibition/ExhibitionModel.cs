﻿using System;
using System.Collections.Generic;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Exhibition
{
    public class ExhibitionModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string City { get; set; }

        public decimal ParticipationCost { get; set; }

        public PaymentMethod ParticipationCostMethod { get; set; }

        public decimal Profit { get; set; }

        public string Notes { get; set; }

        public IEnumerable<ExhibitionProductModel> Products { get; set; }
    }
}