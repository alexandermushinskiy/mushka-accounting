using System;
using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Product
{
    public class ExportRequestModel
    {
        public string Title { get; set; }

        public IEnumerable<Guid> ProductIds { get; set; }
    }
}