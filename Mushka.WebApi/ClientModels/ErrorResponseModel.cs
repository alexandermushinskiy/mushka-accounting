using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels
{
    public class ErrorResponseModel
    {
        public IEnumerable<string> Errors { get; set; }
    }
}