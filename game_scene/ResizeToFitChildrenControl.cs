using Godot;

namespace IndieGameDev.Game.UI;

public enum SizeMode
{
    Off,
    MinimumSize,
    ActualSize
}

/// <summary>
/// This <href="Control"> will resize itself to fit it's children.
/// It can be configured to use the minimum size of it's children
/// or their actual size, and can set the minimum or actual size of this node.
/// Using minimum size is reccommended whenever possible, but actual size
/// may be helpful when resizing for dynamic text labels or any other dynamicly
/// set content.
/// </summary>
[Tool]
public partial class ResizeToFitChildrenControl : Control
{
    [Export] private SizeMode _setXMode;
    [Export] private SizeMode _setYMode;
    [Export] private SizeMode _useChildXMode;
    [Export] private SizeMode _useChildYMode;
    [ExportToolButton("Recalculate Size")] private Callable _ { get => new(this, nameof(RecalculateSize)); }

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;

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

            float childDimensionsX = _useChildXMode switch
            {
                SizeMode.MinimumSize => controlChild.Position.X + controlChild.CustomMinimumSize.X,
                SizeMode.ActualSize => controlChild.Position.X + controlChild.Size.X,
                _ => 0
            };
            float childDimensionsY = _useChildYMode switch
            {
                SizeMode.MinimumSize => controlChild.Position.Y + controlChild.CustomMinimumSize.Y,
                SizeMode.ActualSize => controlChild.Position.Y + controlChild.Size.Y,
                _ => 0
            };

            if (childDimensionsX > newControlSize.X) newControlSize.X = childDimensionsX;
            if (childDimensionsY > newControlSize.Y) newControlSize.Y = childDimensionsY;
        }

        CallDeferred(nameof(ResizeTo), newControlSize);
    }

    private void ResizeTo(Vector2 newControlSize)
    {
        if (_setXMode == SizeMode.MinimumSize && CustomMinimumSize.X != newControlSize.X)
        {
            GD.Print($"Setting X component of custom minimum size of {Name} to {newControlSize.X}");
            CustomMinimumSize = new Vector2(newControlSize.X, CustomMinimumSize.Y);
        }
        else if (_setXMode == SizeMode.ActualSize && Size.X != newControlSize.X)
        {
            GD.Print($"Setting X componont of size of {Name} to {newControlSize.X}");
            Size = new Vector2(newControlSize.X, Size.Y);
        }

        if (_setYMode == SizeMode.MinimumSize && CustomMinimumSize.Y != newControlSize.Y)
        {
            GD.Print($"Setting Y component of custom minimum size of {Name} to {newControlSize.Y}");
            CustomMinimumSize = new Vector2(CustomMinimumSize.X, newControlSize.Y);
        }
        else if (_setYMode == SizeMode.ActualSize && Size.Y != newControlSize.Y)
        {
            GD.Print($"Setting Y componont of size of {Name} to {newControlSize.Y}");
            Size = new Vector2(Size.X, newControlSize.Y);
        }
    }
}
