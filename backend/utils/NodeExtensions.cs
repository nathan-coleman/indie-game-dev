using System;
using Godot;

namespace NathanColeman.IndieGameDev.Utils;

public static class NodeExtensions
{
    public static void FreeChildren(this Node parent)
    {
        ArgumentNullException.ThrowIfNull(parent);

        foreach (var child in parent.GetChildren())
        {
            child.Free();
        }
    }

    public static void QueueFreeChildren(this Node parent)
    {
        ArgumentNullException.ThrowIfNull(parent);

        foreach (var child in parent.GetChildren())
        {
            child.QueueFree();
        }
    }

    public static Godot.Collections.Array<T> GetChildrenOfType<[MustBeVariant] T>(this Node parent)
    {
        ArgumentNullException.ThrowIfNull(parent);

        var childrenOfSpecifiedType = new Godot.Collections.Array<T>();

        foreach (var child in parent.GetChildren())
        {
            if (child is not T childOfSpecifiedType) continue;
            childrenOfSpecifiedType.Add(childOfSpecifiedType);
        }

        return childrenOfSpecifiedType;
    }
}
