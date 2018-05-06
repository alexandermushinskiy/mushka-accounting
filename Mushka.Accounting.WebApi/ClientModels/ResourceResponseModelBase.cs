using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mushka.Accounting.WebApi.ClientModels
{
    public abstract class ResourceResponseModelBase
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<MessageResponseModel> Messages { get; set; }
    }
}