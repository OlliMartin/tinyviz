namespace TinyViz.Templating.Internal;

public record GraphNode
{
    public GraphNode Parent { get; init; }

    public IReadOnlyList<GraphNode>? Children { get; init; }

    public SerializationHint SerializationHint { get; init; } = SerializationHint.WasMap;

    public virtual void SerializeInto(List<object?> list)
    {
        foreach (var child in Children ?? [])
        {
            child.SerializeInto(list);
        }
    }

    public virtual void SerializeInto(Dictionary<string, object?> dictionary)
    {
        foreach (var child in Children ?? [])
        {
            child.SerializeInto(dictionary);
        }
    }
}
