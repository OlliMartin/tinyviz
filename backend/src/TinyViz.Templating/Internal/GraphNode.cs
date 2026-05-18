namespace TinyViz.Templating.Internal;

public record GraphNode
{
    public GraphNode Parent { get; init; }

    public IReadOnlyList<GraphNode>? Children { get; init; }

    public virtual void SerializeInto(Dictionary<string, object?> dictionary)
    {
        foreach (var child in Children ?? [])
        {
            child.SerializeInto(dictionary);
        }
    }
}
