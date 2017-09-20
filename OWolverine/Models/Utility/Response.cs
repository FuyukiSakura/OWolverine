using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;

namespace OWolverine.Models.Utility
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum APIStatus
    {
        Success,
        Fail
    }

    public class Response
    {
        public APIStatus status;
        public string message;
        public IEnumerable data;
    }
}
