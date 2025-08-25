namespace IndieGameDev.Models;

public interface IListItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? IconPath { get; set; }
}
