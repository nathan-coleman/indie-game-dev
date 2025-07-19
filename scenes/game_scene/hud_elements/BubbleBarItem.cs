using Godot;

namespace IndieGameDev.Game.UI;

public partial class BubbleBarItem : Control
{
    public string BubbleName
    {
        get
        {
            var bubbleNameLabel = GetNode<Label>("%BubbleNameLabel");
            return bubbleNameLabel.Text;
        }
        set
        {
            var bubbleNameLabel = GetNode<Label>("%BubbleNameLabel");
            bubbleNameLabel.Text = value;
        }
    }

    public int BubbleAmount
    {
        get
        {
            var bubbleProgressBar = GetNode<ProgressBar>("%BubbleProgressBar");
            return (int)bubbleProgressBar.Value;
        }
        set
        {
            var bubbleProgressBar = GetNode<ProgressBar>("%BubbleProgressBar");
            bubbleProgressBar.Value = value;
        }
    }

    public string BubbleDescription
    {
        get
        {
            return TooltipText;
        }
        set
        {
            var bubbleProgressBar = GetNode<ProgressBar>("%BubbleProgressBar");
            bubbleProgressBar.TooltipText = value;
            TooltipText = value;
        }
    }
}
