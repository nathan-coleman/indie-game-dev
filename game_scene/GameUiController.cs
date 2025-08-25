using Godot;
using NathanColeman.IndieGameDev.Backend;

namespace NathanColeman.IndieGameDev.Ui;

public partial class GameUiController : Node
{
    public void SetMenuBar(ProjectType currentProject)
    {
        var topLayoutBox = GetNode<TabContainer>("../TopLayoutBox");
        topLayoutBox.CurrentTab = (int)currentProject;
    }

    public void SetGameName(string currentGameName)
    {
        GetNode<Label>("%GameNameLabel").Text = currentGameName;
    }
}
