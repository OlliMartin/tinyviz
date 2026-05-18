namespace TinyViz.Templating.Internal;

public record GraphNode
{
    public GraphNode Parent { get; init; }

    public IReadOnlyList<GraphNode>? Children { get; init; }
}
