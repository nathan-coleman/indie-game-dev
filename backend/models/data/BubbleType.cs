using Newtonsoft.Json;

namespace NathanColeman.IndieGameDev.Models;

public record BubbleType
{
    [JsonProperty(Required = Required.Always)]
    public required string Name { get; set; }
    [JsonProperty(Required = Required.Always)]
    public required string Description { get; set; }
    public string? Color { get; set; }
    public string? TechnologyPrerequisite { get; set; }
}
