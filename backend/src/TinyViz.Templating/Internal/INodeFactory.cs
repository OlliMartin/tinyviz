namespace TinyViz.Templating.Internal;

public interface INodeFactory
{
    int Priority { get; }

    bool Handles(object? item, object? context = null);

    GraphNode CreateNode(
        Func<GraphNode, object?, object?, GraphNode> childrenFactory,
        GraphNode parent,
        object? item,
        object? context = null
    );
}
