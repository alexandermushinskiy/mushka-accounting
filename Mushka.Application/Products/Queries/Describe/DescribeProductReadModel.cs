using System;

namespace Mushka.Application.Products.Queries.Describe;

public sealed class DescribeProductReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VendorCode { get; set; }

    public decimal? RecommendedPrice { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CategoryId { get; set; }

    public DescribeProductCategoryReadModel Category { get; set; }

    public DescribeProductSizeReadModel Size { get; set; }

    public bool IsArchival { get; set; }
}

public sealed class DescribeProductCategoryReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}

public sealed class DescribeProductSizeReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}
