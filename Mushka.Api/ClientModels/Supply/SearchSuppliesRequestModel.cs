using System;

namespace Mushka.Api.ClientModels.Supply;

public class SearchSuppliesRequestModel
{
    public string SearchKey { get; set; }

    public Guid? ProductId { get; set; }
}
