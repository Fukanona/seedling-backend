using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Model
{
    public class ProblemDto : ITable
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; }

        public string Path
        {
            get => nameof(ProblemDto).ToLower();
        }
    }

    public enum ProblemLevel {
        EASY,
        MEDIUM,
        HARD,
        INSANE
    }
}
