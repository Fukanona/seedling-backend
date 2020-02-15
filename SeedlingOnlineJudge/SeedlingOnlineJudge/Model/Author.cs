using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Model
{
    public class Author
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        public void Copy(Author otherAuthor)
        {
            Company = (string.IsNullOrEmpty(otherAuthor?.Company) ? Company : otherAuthor.Company);
            Country = (string.IsNullOrEmpty(otherAuthor?.Country) ? Country : otherAuthor.Country);
            Username = (string.IsNullOrEmpty(otherAuthor?.Username) ? Username : otherAuthor.Username);
            Name = (string.IsNullOrEmpty(otherAuthor?.Name) ? Name : otherAuthor.Name);
            State = (string.IsNullOrEmpty(otherAuthor?.State) ? State : otherAuthor.State);
        }

        public void Copy(User otherUser)
        {
            Company = (string.IsNullOrEmpty(otherUser?.Company) ? Company : otherUser.Company);
            Country = (string.IsNullOrEmpty(otherUser?.Country) ? Country : otherUser.Country);
            Username = (string.IsNullOrEmpty(otherUser?.Username) ? Username : otherUser.Username);
            Name = (string.IsNullOrEmpty(otherUser?.Name) ? Name : otherUser.Name);
            State = (string.IsNullOrEmpty(otherUser?.State) ? State : otherUser.State);
        }
    }
}
