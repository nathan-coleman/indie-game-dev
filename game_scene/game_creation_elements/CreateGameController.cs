using Godot;

namespace IndieGameDev.Game.UI;

public partial class CreateGameController : Node
{
    private TabContainer gameOptionSelectionTabContainer;

    public override void _Ready()
    {
        gameOptionSelectionTabContainer = GetNode<TabContainer>("%GameOptionSelectionTabContainer");

        ConnectButtons();
    }

    private void ConnectButtons()
    {
        GetNode<Button>("%OpenGenrePaneButton").Pressed += OpenGenrePane;
        GetNode<Button>("%OpenTopicPaneButton").Pressed += OpenTopicPane;
        GetNode<Button>("%OpenPlatformPaneButton").Pressed += OpenPlatformPane;
        GetNode<Button>("%OpenGraphicsPaneButton").Pressed += OpenGraphicsPane;
        GetNode<Button>("%OpenTargetAudiencePaneButton").Pressed += OpenTargetAudiencePane;
    }

    private void OpenGenrePane() => gameOptionSelectionTabContainer.CurrentTab = 0;
    private void OpenTopicPane() => gameOptionSelectionTabContainer.CurrentTab = 1;
    private void OpenPlatformPane() => gameOptionSelectionTabContainer.CurrentTab = 2;
    private void OpenGraphicsPane() => gameOptionSelectionTabContainer.CurrentTab = 3;
    private void OpenTargetAudiencePane() => gameOptionSelectionTabContainer.CurrentTab = 4;
}
