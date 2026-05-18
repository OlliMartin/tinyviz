using TinyViz.Templating.Internal.Nodes;

namespace TinyViz.Templating.Internal.NodeFactories;

public class NamedNodeFactory : INodeFactory
{
    public int Priority => 10;

    public bool Handles(object? item, object? context = null) =>
        context is not null && item is IList<object?> or IDictionary<string, object?> or IDictionary<object, object?>;

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

        if (item is not IList<object?> and not IDictionary<string, object?> and not IDictionary<object, object?>)
        {
            throw new InvalidOperationException(); // TODO
        }

        var result = new KeyedNode(namedNodeKey);

        var childNode = childrenFactory(result, item, arg3: null);

        return result with
        {
            Children = childNode.Children,
        };
    }
}
