namespace TinyViz.Templating.Internal.Nodes;

public record PrimitiveNode(object? Value) : GraphNode
{
    public override void SerializeInto(List<object?> list) => list.Add(Value);
}
