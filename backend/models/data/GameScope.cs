using Newtonsoft.Json;

namespace NathanColeman.IndieGameDev.Models;

public record GameScope : IListItem
{
    [JsonProperty(Required = Required.Always)]
    public required string Name { get; set; }
    [JsonProperty(Required = Required.Always)]
    public required string Description { get; set; }
    [JsonProperty(Required = Required.Always)]
    public int CompletionTime { get; set; }
    public string? IconPath { get; set; }
    public string? TechnologyPrerequisite { get; set; }
}
