using System;

namespace Mushka.Api.ClientModels.Order;

public class SearchOrdersRequestModel
{
    public SearchOrdersQueryRequestModel Query { get; set; }

    public SearchOrdersNavigationRequestModel Navigation { get; set; }
}

public class SearchOrdersQueryRequestModel
{
    public SearchOrdersCustomerRequestModel Customer { get; set; }

    public SearchOrdersOrderRequestModel Order { get; set; }
}

public class SearchOrdersCustomerRequestModel
{
    public SearchOrdersLikeRequestModel Name { get; set; }
}

public class SearchOrdersOrderRequestModel
{
    public SearchOrdersBetweenRequestModel OrderDate { get; set; }
}

public class SearchOrdersLikeRequestModel
{
    public string Like { get; set; }
}

public class SearchOrdersBetweenRequestModel
{
    public DateTime? From { get; set; }

    public DateTime? To { get; set; }
}

public class SearchOrdersNavigationRequestModel
{
    public SearchOrdersSortRequestModel Sort { get; set; }

    public SearchOrdersPageRequestModel Page { get; set; }
}

public class SearchOrdersSortRequestModel
{
    public string Key { get; set; }

    public string Order { get; set; }
}

public class SearchOrdersPageRequestModel
{
    public int Size { get; set; }

    public int From { get; set; }
}
