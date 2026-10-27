using System.Collections.Generic;

namespace Mushka.Api.ClientModels;

public class ErrorResponseModel
{
    public IEnumerable<string> Errors { get; set; }
}
