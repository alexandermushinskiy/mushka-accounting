using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mushka.Domain.Entities;
using Mushka.WebApi.Filters;

namespace Mushka.WebApi.ClientModels.Exhibition
{
    public class ExhibitionRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public string City { get; set; }

        [RequireNonDefault]
        public decimal ParticipationCost { get; set; }

        [RequireNonDefault]
        public PaymentMethod ParticipationCostMethod { get; set; }
        
        [RequireNonDefault]
        public decimal Profit { get; set; }

        public string Notes { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<ExhibitionProductRequestModel> Products { get; set; }
    }
}