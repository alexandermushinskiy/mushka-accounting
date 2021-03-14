using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Exhibition.Describe
{
    public class DescribeExhibitionResponseModel
    {
        public DescribeExhibitionModel Exhibition { get; set; }

        public IEnumerable<DescribeProductModel> Products { get; set; }
    }
}