using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Model
{
    public class User : PData<User>
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("registerTime")]
        public DateTime RegisterTime { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        public override string GetPDataKey() => $"{Username}";
    }
}
