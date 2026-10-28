using System;
using System.Collections.Generic;

namespace Mushka.Api.ClientModels.Product;

public class ExportProductsRequestModel
{
    public string Title { get; set; }

    public IEnumerable<Guid> ProductIds { get; set; }
}
