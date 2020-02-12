using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Model
{
    public class ProblemDescription : PData<ProblemDescription>
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("author")]
        public User Author { get; set; }

        [JsonPropertyName("competition")]
        public string Competition { get; set; }

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; }

        public override string GetPDataKey() => $"{Id}";
        
    }

    public enum ProblemLevel
    {
        EASY,
        MEDIUM,
        HARD,
        INSANE
    }

    public enum ProblemCategory
    {
        ADHOC,
        STRING,
        MATH,
        GEOMETRY,
        STRUCTURE,
        BEGINNER,
        GRAPH,
        PARADIGM
    }
}
