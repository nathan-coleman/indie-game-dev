using System.Collections.Generic;
using Godot;

namespace IndieGameDev.Game.UI;

public partial class BubblesBox : Node
{
    [Export] private PackedScene _bubbleBarItemPrefab;

    private static readonly List<(string name, int amount, string description)> demoBubbleValues = new()
    {
        ("Story", 10, "Test Description"),
        ("Backend", 20, "Test Description"),
        ("Gameplay", 40, "Test Description"),
        ("Sound", 80, "Test Description"),
        ("Progression", 50, "Test Description"),
        ("Hype", 70, "Test Description"),
        ("Polish", 30, "Test Description")
    };

    public override void _Ready()
    {
        var bubbleBarContainer = GetNode<Control>("%BubbleBarContainer");

        foreach (var childNode in bubbleBarContainer.GetChildren())
        {
            if (childNode is not BubbleBarItem) continue;
            childNode.QueueFree();
        }

        foreach (var bubbleValue in demoBubbleValues)
        {
            var newBubbleBarItem = _bubbleBarItemPrefab.Instantiate<BubbleBarItem>();

            newBubbleBarItem.BubbleName = bubbleValue.name;
            newBubbleBarItem.BubbleAmount = bubbleValue.amount;
            newBubbleBarItem.BubbleDescription = bubbleValue.description;

            bubbleBarContainer.AddChild(newBubbleBarItem);
        }
    }
}
