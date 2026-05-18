namespace TinyViz.Templating.Internal.Nodes;

public record NamedPrimitiveNode(string Name, object? Value) : NamedNode(Name) { }
