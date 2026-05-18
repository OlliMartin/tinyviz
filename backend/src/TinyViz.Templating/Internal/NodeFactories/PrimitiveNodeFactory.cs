using TinyViz.Templating.Internal.Nodes;

namespace TinyViz.Templating.Internal.NodeFactories;

public class PrimitiveNodeFactory : INodeFactory
{
    public int Priority => 0;

    public bool Handles(object? item, object? context = null) => TypeUtils.IsPrimitive(item);

    public GraphNode CreateNode(
        Func<GraphNode, object?, object?, GraphNode> childrenFactory,
        GraphNode parent,
        object? item,
        object? context = null
    ) => new PrimitiveNode(item);
}
