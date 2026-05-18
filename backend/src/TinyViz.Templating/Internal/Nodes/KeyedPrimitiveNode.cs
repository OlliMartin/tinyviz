namespace TinyViz.Templating.Internal.Nodes;

public record KeyedPrimitiveNode(string Key, object? Value) : KeyedNode(Key);
