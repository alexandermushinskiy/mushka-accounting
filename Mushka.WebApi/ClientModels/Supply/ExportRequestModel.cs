using System;
using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class ExportRequestModel
    {
        public string Title { get; set; }

        public IEnumerable<Guid> SupplyIds { get; set; }

        public IEnumerable<Guid> ProductIds { get; set; }
    }
}