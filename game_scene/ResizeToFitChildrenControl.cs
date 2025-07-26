using Godot;

namespace IndieGameDev.Game.UI;

/// <summary>
/// This <href="Control"> will resize itself to fit it's children.
/// It can be configured to use the minimum size of it's children
/// or their actual size, and can set the minimum or actual size of this node.
/// Using minimum size is reccommended whenever possible, but actual size
/// may be helpful when resizing for dynamic text labels or any other dynamicly
/// set content.
/// </summary>
public partial class ResizeToFitChildrenControl : Control
{
    [Export] private bool _setMinimumSize;
    [Export] private bool _setSize;
    [Export] private bool _useChildMinimumSize;
    [Export] private bool _useChildSize;

    public override void _Ready()
    {
        if ((_useChildMinimumSize && _useChildSize)
            || (!_useChildMinimumSize && !_useChildSize))
        {
            GD.Print($"{Name} Is not configured correctly and so will not resize itself.\n"
                + $"Exactly one {nameof(_useChildMinimumSize)} or {nameof(_useChildSize)} must be true.");

            return;
        }

        foreach (Node child in GetChildren())
        {
            OnChildEnteredTree(child);
        }

        ChildEnteredTree += OnChildEnteredTree;
        ChildExitingTree += OnChildExitingTree;
    }

    private void OnChildEnteredTree(Node child)
    {
        if (child is not Control controlChild) return;
        controlChild.Resized += RecalculateSize;
        controlChild.MinimumSizeChanged += RecalculateSize;
    }

    private void OnChildExitingTree(Node child)
    {
        if (child is not Control) return;
        // We do not unsubscribe from events here because Godot does that automatically.
        RecalculateSize();
    }

    private void RecalculateSize()
    {
        Vector2 newControlSize = new();

        foreach (var child in GetChildren())
        {
            if (child is not Control controlChild) continue;

            Vector2 childDimensions = _useChildSize
                ? controlChild.Position + controlChild.Size
                : controlChild.Position + controlChild.CustomMinimumSize;

            if (childDimensions.X > newControlSize.X) newControlSize.X = childDimensions.X;
            if (childDimensions.Y > newControlSize.Y) newControlSize.Y = childDimensions.Y;
        }

        // Is there a way to cancel deffered calls?
        // CancelDeferred(nameof(ResizeTo));

        CallDeferred(nameof(ResizeTo), newControlSize);
    }

    private void ResizeTo(Vector2 newControlSize)
    {
        if (_setMinimumSize && CustomMinimumSize != newControlSize)
        {
            GD.Print($"Setting custom minimum size of {Name} to {newControlSize}");
            CustomMinimumSize = newControlSize;
        }

        if (_setSize && Size != newControlSize)
        {
            GD.Print($"Setting size of {Name} to {newControlSize}");
            Size = newControlSize;
        }
    }
}
