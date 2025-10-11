using Godot;
using NathanColeman.IndieGameDev.Backend;

namespace NathanColeman.IndieGameDev.Ui;

public partial class CompleteGameController : Node
{
    public override void _Ready()
    {
        ConnectButtons();
    }

    public void SetScore(float score)
    {
        var gameScoreLabel = GetNode<Label>("%GameScoreLabel");
        gameScoreLabel.Text = $"Your game achieved a score of {score:F2}!";
    }

    private void ConnectButtons()
    {
        GetNode<Button>("%CloseButton").Pressed += CloseMenu;
    }

    private void CloseMenu()
    {
        GameController.Instance.GameUiController.HideCenterContent();
    }
}
