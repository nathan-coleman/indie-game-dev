namespace IndieGameDev.Models;

public record GameScope : IListItemModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CompletionTime { get; set; }
    public string IconPath { get; set; }
    public string TechnologyPrerequisite { get; set; }
}
