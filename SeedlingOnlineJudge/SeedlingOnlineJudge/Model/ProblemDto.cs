using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeedlingOnlineJudge.Model
{
    public class ProblemDto : Table<ProblemDto>
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; }

        public override string GetTableKey() => $"{Id}";
        
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
        BEGINNER
    }
}
