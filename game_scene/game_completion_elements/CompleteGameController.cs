using Godot;

namespace NathanColeman.IndieGameDev.Ui;

public partial class CompleteGameController : Node
{
    public override void _Ready()
    {
        ConnectButtons();
    }

    public void SetScore(float score)
    {
        var dateDisplayLabel = GetNode<Label>("%GameScoreLabel");
        dateDisplayLabel.Text = $"Your game achieved a score of {score:F2}!";
    }

    private void ConnectButtons()
    {
        GetNode<Button>("%CloseButton").Pressed += CloseMenu;
    }

    private void CloseMenu()
    {
        // center ui layout controller and call Close, same as default CloseButton
    }
}
