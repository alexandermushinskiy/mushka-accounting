using System;
using System.Collections.Generic;

namespace Mushka.Api.ClientModels.Supply;

public class ExportSuppliesRequestModel
{
    public string Title { get; set; }

    public IEnumerable<Guid> SupplyIds { get; set; }

    public IEnumerable<Guid> ProductIds { get; set; }
}
