using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mushka.WebApi.ClientModels
{
    public class ResponseModelBase
    {
        [JsonIgnore]
        public int? StatusCode { get; set; }

        public IEnumerable<string> Messages { get; set; }
    }
}