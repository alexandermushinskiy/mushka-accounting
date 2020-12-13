using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mushka.WebApi.ClientModels
{
    public class ResponseModelBaseOld
    {
        [JsonIgnore]
        public int? StatusCode { get; set; }

        public IEnumerable<string> Messages { get; set; }
    }



    public class ResponseModelBase
    {
        [JsonIgnore]
        public int? StatusCode { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }

    public class ResponseModelBase<TResponseData> : ResponseModelBase
    {
        public TResponseData Data { get; set; }
    }

    public class ResponseModelListBase<TResponseData> : ResponseModelBase<IEnumerable<TResponseData>>
    {
    }
}