using System.Collections.Generic;
using Newtonsoft.Json;

namespace IndieGameDev.Models;

public record GameGenre : IListItem
{
    [JsonProperty(Required = Required.Always)]
    public required string Name { get; set; }
    [JsonProperty(Required = Required.Always)]
    public required string Description { get; set; }
    [JsonProperty(Required = Required.Always)]
    public required IReadOnlyDictionary<string, float> BubbleEffectiveness { get; set; }
    public string? IconPath { get; set; }
    public string? ParentGenre { get; set; }
    public string? TechnologyPrerequisite { get; set; }

    [JsonIgnore]
    public bool IsSubGenre { get => !string.IsNullOrEmpty(ParentGenre); }
}
