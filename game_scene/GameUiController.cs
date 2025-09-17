using Godot;
using NathanColeman.IndieGameDev.Backend;
using NathanColeman.IndieGameDev.Utils;
using NathanColeman.IndieGameDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NathanColeman.IndieGameDev.Ui;

public partial class GameUiController : Node
{
    [Export] private PackedScene? _bubbleBarItemPrefab;

    public void SetMenuBar(ProjectType currentProject)
    {
        var topLayoutBox = GetNode<TabContainer>("../TopLayoutBox");
        topLayoutBox.CurrentTab = (int)currentProject;
    }

    public void SetGameName(string currentGameName)
    {
        GetNode<Label>("%GameNameLabel").Text = currentGameName;
    }

    public void InitializeBubblesBox()
    {
        ArgumentNullException.ThrowIfNull(_bubbleBarItemPrefab);

        var bubbleBarContainer = GetNode<Control>("%BubbleBarContainer");
        bubbleBarContainer.QueueFreeChildren();

        var bubbleTypes = GameController.Instance.DataLoader.LoadData<List<BubbleType>>("BubbleTypes");

        foreach (var bubbleType in bubbleTypes)
        {
            var newBubbleBarItem = _bubbleBarItemPrefab.Instantiate<BubbleBarItem>();

            newBubbleBarItem.BubbleName = bubbleType.Name;
            newBubbleBarItem.BubbleAmount = 0;
            newBubbleBarItem.BubbleDescription = bubbleType.Description;

            bubbleBarContainer.AddChild(newBubbleBarItem);
        }
    }

    public void AddBubbles(string bubbleName, int amount)
    {
        var bubbleBarContainer = GetNode<Control>("%BubbleBarContainer");
        var bubbleBarItems = bubbleBarContainer.GetChildrenOfType<BubbleBarItem>();
        var itemToUpdate = bubbleBarItems.FirstOrDefault(item => item.BubbleName == bubbleName);

        if (itemToUpdate == null)
        {
            GD.PrintErr($"Could not add {amount} bubbles to '{bubbleName}': a bubble bar of type '{bubbleName}' could not be found.");
            return;
        }

        itemToUpdate.BubbleAmount += amount;
    }
}
