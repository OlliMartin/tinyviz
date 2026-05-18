using TinyViz.Templating.Internal.Nodes;

namespace TinyViz.Templating.Internal.NodeFactories;

public class KeyPrimitiveValueNodeFactory : INodeFactory
{
    public int Priority => 15;

    public bool Handles(object? item, object? context = null) => context is string && TypeUtils.IsPrimitive(item);

    public GraphNode CreateNode(
        Func<GraphNode, object?, object?, GraphNode> childrenFactory,
        GraphNode parent,
        object? item,
        object? context = null
    )
    {
        var namedNodeKey = context?.ToString();

        if (namedNodeKey is null)
        {
            throw new InvalidOperationException(
                "Invalid application state. Context expected to be string because it passed handles, but it's not."
            );
        }

        if (TypeUtils.IsPrimitive(item) is false)
        {
            throw new InvalidOperationException(); // TODO
        }

        var result = new NamedPrimitiveNode(namedNodeKey, item);

        return result;
    }
}
