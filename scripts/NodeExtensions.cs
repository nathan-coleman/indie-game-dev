using System;
using System.Diagnostics.CodeAnalysis;
using Godot;

namespace IndieGameDev.Utils;

public static class NodeExtensions
{
    public static bool TryGetNode<T>(this Node thisNode, NodePath nodeName, [NotNullWhen(true)] out T resultNode)
        where T : Node
    {
        ArgumentNullException.ThrowIfNull(thisNode);

        resultNode = thisNode.GetNodeOrNull<T>(nodeName);
        return resultNode != null;
    }
}
