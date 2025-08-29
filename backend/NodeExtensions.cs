using System;
using Godot;

namespace NathanColeman.IndieGameDev.Utils;

public static class NodeExtensions
{
    public static void FreeChildren(this Node thisNode)
    {
        ArgumentNullException.ThrowIfNull(thisNode);

        foreach (var child in thisNode.GetChildren())
        {
            child.Free();
        }
    }

    public static void QueueFreeChildren(this Node thisNode)
    {
        ArgumentNullException.ThrowIfNull(thisNode);

        foreach (var child in thisNode.GetChildren())
        {
            child.QueueFree();
        }
    }
}
