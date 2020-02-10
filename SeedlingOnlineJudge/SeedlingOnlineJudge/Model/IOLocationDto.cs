using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Model
{
    public class IOLocationDto : PData<IOLocationDto>
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("in")]
        public string In { get; set; }

        [JsonPropertyName("out")]
        public string Out { get; set; }

        public override string GetPDataKey() => $"{Id}";
    }
}
