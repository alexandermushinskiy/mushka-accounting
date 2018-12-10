using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mushka.WebApi.ClientModels
{
    public class ResponseModelBase
    {
        [JsonIgnore]
        public int? StatusCode { get; set; }

        public IEnumerable<ResponseMessageModel> Messages { get; set; }
    }

    public class ResponseMessageModel
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}