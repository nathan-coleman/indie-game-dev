using Newtonsoft.Json;

namespace IndieGameDev.Models;

public record GameScope : IListItemModel
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
