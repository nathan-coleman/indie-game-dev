using System.Collections.Generic;
using Newtonsoft.Json;

namespace NathanColeman.IndieGameDev.Models;

public record GameTopic : IListItem
{
    [JsonProperty(Required = Required.Always)]
    public required string Name { get; set; }
    [JsonProperty(Required = Required.Always)]
    public required string Description { get; set; }
    [JsonProperty(Required = Required.Always)]
    public required Dictionary<string, float> AudienceAffinities { get; set; }
    [JsonProperty(Required = Required.Always)]
    public required Dictionary<string, float> GenreAffinities { get; set; }
    public string? IconPath { get; set; }
    public string? TechnologyPrerequisite { get; set; }
}
