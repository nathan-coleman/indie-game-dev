using System.Collections.Generic;
using Newtonsoft.Json;

namespace IndieGameDev.Models;

public record GameTopic : IListItemModel
{
    [JsonProperty(Required = Required.Always)]
    public required string Name { get; set; }
    [JsonProperty(Required = Required.Always)]
    public required string Description { get; set; }
    [JsonProperty(Required = Required.Always)]
    public required IReadOnlyDictionary<string, float> BubbleEffectiveness { get; set; }
    public string? IconPath { get; set; }
    public string? TechnologyPrerequisite { get; set; }
}
