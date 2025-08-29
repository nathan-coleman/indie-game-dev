using System;
using Godot;
using NathanColeman.Addons.ResizeToFitChildrenControl;

namespace NathanColeman.IndieGameDev.Ui;

[Tool]
public partial class LargeGridItem : ResizeToFitChildrenControl
{
    [Export]
    public string? ItemName
    {
        get => GetNodeOrNull<Label>("%ItemName")?.Text;
        set
        {
            var itemName = GetNodeOrNull<Label>("%ItemName");
            if (itemName != null)
            {
                itemName.Text = value;
            }
        }
    }

    [Export]
    public Texture2D? ItemIcon
    {
        get => GetNodeOrNull<TextureRect>("%ItemIcon")?.Texture;
        set
        {
            var itemIcon = GetNodeOrNull<TextureRect>("%ItemIcon");
            if (itemIcon != null)
            {
                itemIcon.Texture = value;
            }
        }
    }

    [Export(PropertyHint.MultilineText)]
    public string ItemDescription
    {
        get => TooltipText;
        set => TooltipText = value;
    }

    public event Action Pressed
    {
        add => GetNode<Button>("Button").Pressed += value;
        remove => GetNode<Button>("Button").Pressed -= value;
    }
}
