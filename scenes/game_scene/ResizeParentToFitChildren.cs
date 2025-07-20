using Godot;

namespace IndieGameDev.Game.UI;

public partial class ResizeParentToFitChildren : Node
{
    [Export] private bool _setMinimumSize;
    [Export] private bool _setSize;
    [Export] private bool _useSiblingActualSize;

    private Control _parent;

    public override void _Ready()
    {
        if (GetParent() is not Control parent)
        {
            GD.PushError($"{nameof(ResizeParentToFitChildren)} must be child of {nameof(Control)} node.");
            return;
        }
        _parent = parent;

        foreach (Node sibling in _parent.GetChildren())
        {
            OnSiblingEnteredTree(sibling);
        }

        _parent.ChildEnteredTree += OnSiblingEnteredTree;
        _parent.ChildExitingTree += OnSiblingExitingTree;
    }

    private void OnSiblingEnteredTree(Node sibling)
    {
        if (sibling is not Control controlSibling) return;
        controlSibling.Resized += RecalculateParentSize;
        controlSibling.MinimumSizeChanged += RecalculateParentSize;
    }

    private void OnSiblingExitingTree(Node sibling)
    {
        if (sibling is not Control) return;
        RecalculateParentSize();
    }

    private void RecalculateParentSize()
    {
        Vector2 newParentSize = new();

        foreach (var sibling in _parent.GetChildren())
        {
            if (sibling is not Control controlSibling) continue;

            Vector2 siblingDimensions = _useSiblingActualSize
                ? controlSibling.Position + controlSibling.Size
                : controlSibling.Position + controlSibling.CustomMinimumSize;

            if (siblingDimensions.X > newParentSize.X) newParentSize.X = siblingDimensions.X;
            if (siblingDimensions.Y > newParentSize.Y) newParentSize.Y = siblingDimensions.Y;
        }

        CallDeferred(nameof(ResizeParent), newParentSize);
    }

    private void ResizeParent(Vector2 newParentSize)
    {
        if (_setMinimumSize && _parent.CustomMinimumSize != newParentSize)
        {
            _parent.CustomMinimumSize = newParentSize;
            GD.Print($"Setting custom minimum size of {_parent.Name} to {newParentSize}");
        }

        if (_setSize && _parent.Size != newParentSize)
        {
            GD.Print($"Setting size of {_parent.Name} to {newParentSize}");
            _parent.Size = newParentSize;
        }
    }
}
