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

        public void Copy(User otherUser)
        {
            Company = (string.IsNullOrEmpty(otherUser?.Company) ? Company : otherUser.Company);
            Country = (string.IsNullOrEmpty(otherUser?.Country) ? Country : otherUser.Country);
            Name = (string.IsNullOrEmpty(otherUser?.Name) ? Name : otherUser.Name);
            Password = (string.IsNullOrEmpty(otherUser?.Password) ? Password : otherUser.Password);
            State = (string.IsNullOrEmpty(otherUser?.State) ? State : otherUser.State);
        }
    }
}
