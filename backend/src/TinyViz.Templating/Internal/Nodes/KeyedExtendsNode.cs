namespace TinyViz.Templating.Internal.Nodes;

public record KeyedExtendsNode(string Key, string Namespace, string Name) : ExtendsNode(Namespace, Name), IKeyedNode;
