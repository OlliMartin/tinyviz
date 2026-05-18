namespace TinyViz.Templating.Internal.NodeFactories;

public class ExtendsNodeFactory : INodeFactory
{
    public int Priority => 100;

    public bool Handles(object? item, object? context = null) => false;

    public GraphNode CreateNode(
        Func<GraphNode, object?, object?, GraphNode> childrenFactory,
        GraphNode parent,
        object? item,
        object? context = null
    ) => throw new NotImplementedException();
}
