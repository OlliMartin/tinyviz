namespace TinyViz.Templating.Internal.Nodes;

public record KeyedNode(string Key) : GraphNode, IKeyedNode;
