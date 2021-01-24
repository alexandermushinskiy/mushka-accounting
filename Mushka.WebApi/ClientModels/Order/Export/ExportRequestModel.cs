using System;
using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Order.Export
{
    public class ExportRequestModel
    {
        public string Title { get; set; }

        public IEnumerable<Guid> OrderIds { get; set; }
    }
}