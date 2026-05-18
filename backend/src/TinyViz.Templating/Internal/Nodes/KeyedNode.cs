namespace TinyViz.Templating.Internal.Nodes;

public record KeyedNode(string Key) : GraphNode, IKeyedNode
{
    public override void SerializeInto(Dictionary<string, object?> dictionary)
    {
        if (IsLeaf)
        {
            if (SerializationHint == SerializationHint.WasMap)
            {
                dictionary.Add(Key, new Dictionary<string, object?>());
            }
            else if (SerializationHint is SerializationHint.WasList)
            {
                dictionary.Add(Key, new List<object?>());
            }

            return;
        }

        if (AllChildrenKeyed)
        {
            var nextDict = new Dictionary<string, object?>();
            dictionary.Add(Key, nextDict);

            foreach (var child in Children)
            {
                child.SerializeInto(nextDict);
            }

            return;
        }

        if (Children.Count(c => c is not IKeyedNode) != Children.Count)
        {
            throw new InvalidOperationException(
                "Invalid application state: Expected the children of any given node to be exclusively either keyed or all not keyed. "
                    + "Instead, found a mixture of keyed and non-keyed nodes, which indicates a bug in the node processing."
            );
        }

        var nextList = new List<object?>();
        dictionary.Add(Key, nextList);

        foreach (var child in Children)
        {
            child.SerializeInto(nextList);
        }
    }
}
