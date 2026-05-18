namespace TinyViz.Templating.Internal;

public record GraphNode
{
    public virtual string Type { get; } = "$implicit";

    public GraphNode Parent { get; init; }

    public IReadOnlyList<GraphNode>? Children { get; init; }
}
