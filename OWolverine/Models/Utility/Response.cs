using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Runtime.Serialization;

namespace OWolverine.Models.Utility
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum APIStatus
    {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "fail")]
        Fail
    }

    public class Response
    {
        public APIStatus status;
        public string message;
        public IEnumerable data;
    }
}
