using System.Diagnostics.CodeAnalysis;

namespace TinyViz.Templating.Internal;

public record GraphNode
{
    public GraphNode Parent { get; init; }

    public IReadOnlyList<GraphNode>? Children { get; init; }

    public SerializationHint SerializationHint { get; init; } = SerializationHint.WasMap;

    [MemberNotNullWhen(returnValue: false, nameof(Children))]
    protected bool IsLeaf => Children?.Count is not >= 1;

    protected bool AllChildrenKeyed => IsLeaf is false && Children.All(c => c is IKeyedNode);

    public virtual void SerializeInto(List<object?> list)
    {
        if (IsLeaf)
        {
            return;
        }

        if (AllChildrenKeyed)
        {
            var nextDict = new Dictionary<string, object?>();

            foreach (var child in Children)
            {
                child.SerializeInto(nextDict);
            }

            list.Add(nextDict);
            return;
        }

        // TODO: Check that either all are keyed or none.. If not -> programming error.

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
