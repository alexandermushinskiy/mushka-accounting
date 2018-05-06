using System;
using Newtonsoft.Json;
using LevelTypeEnum = Mushka.Accounting.Core.Validation.Enums.LevelType;

namespace Mushka.Accounting.WebApi.ClientModels
{
    public class MessageResponseModel
    {
        [Obsolete("Created for AutoMapper", true)]
        public MessageResponseModel()
        {
        }

        private MessageResponseModel(string levelType, string message, int? statusCode = null)
        {
            LevelType = levelType;
            Message = message;
            StatusCode = statusCode;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LevelType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? StatusCode { get; set; }

        public string Message { get; set; }

        public static MessageResponseModel Warning(string message, int? statusCode = null) =>
            new MessageResponseModel(LevelTypeEnum.Warning.ToString(), message, statusCode);
    }
}