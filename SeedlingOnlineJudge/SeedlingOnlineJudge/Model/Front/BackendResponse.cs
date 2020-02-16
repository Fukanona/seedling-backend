using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Model.Front
{
    public class BackendResponse<T> where T : class
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("responseTime")]
        public DateTime ResponseTime { get; set; }

        [JsonPropertyName("statusCode")]
        public string StatusCode { get; set; }

        [JsonPropertyName("statusCodeDescription")]
        public string StatusCodeDescription { get; set; }

        [JsonPropertyName("content")]
        public T Content { get; set; }
    }
}
