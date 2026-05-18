namespace TinyViz.Templating.Internal;

internal class DefaultTemplatingEngine(IEnumerable<INodeFactory> nodeFactories) : ITemplatingEngine
{
    private IList<INodeFactory> OrderedNodeFactories { get; } = nodeFactories.OrderByDescending(f => f.Priority).ToList();

    public async Task<TTemplate> RenderTemplateAsync<TTemplate>(
        IEnumerable<ITemplateProvider> templateProviders,
        TTemplate templatable,
        CancellationToken cancellationToken = default
    )
        where TTemplate : ITemplate
    {
        var graph = CreateGraph(templatable);

        return templatable;
    }

    internal GraphNode CreateGraph(ITemplate templatable)
    {
        var node = GenerateGraph(null!, templatable.Content);
        node = node with { Parent = node };

        return node;
    }

    private GraphNode GenerateGraph(GraphNode parent, object? content, object? context = null)
    {
        var factory = OrderedNodeFactories.FirstOrDefault(f => f.Handles(content, context));

        var node = factory?.CreateNode(GenerateGraph, parent, content, context) ?? new GraphNode();

        var children = new List<GraphNode>();

        if (content is Dictionary<object, object?> dict)
        {
            foreach (var kvp in dict)
            {
                children.Add(GenerateGraph(node, kvp.Value, kvp.Key));
            }
        }
        else if (content is Dictionary<string, object?> keyedDict)
        {
            foreach (var kvp in keyedDict)
            {
                children.Add(GenerateGraph(node, kvp.Value, kvp.Key));
            }
        }
        else if (content is IList<object?> list)
        {
            foreach (var item in list)
            {
                children.Add(GenerateGraph(node, item));
            }
        }

        return node with
        {
            Children = node.Children ?? children,
            Parent = parent,
        };
    }
}
