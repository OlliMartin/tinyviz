namespace TinyViz.Templating.Internal.Nodes;

public record KeyedPrimitiveNode(string Key, object? Value) : KeyedNode(Key)
{
    public override void SerializeInto(Dictionary<string, object?> dictionary) => dictionary.Add(Key, Value);
}
