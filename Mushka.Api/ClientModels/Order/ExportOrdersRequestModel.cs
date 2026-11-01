using System;
using System.Collections.Generic;

namespace Mushka.Api.ClientModels.Order;

public class ExportOrdersRequestModel
{
    public string Title { get; set; }

    public IEnumerable<Guid> OrderIds { get; set; }
}
