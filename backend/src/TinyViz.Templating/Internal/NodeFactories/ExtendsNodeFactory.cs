using TinyViz.Templating.Internal.Nodes;

namespace TinyViz.Templating.Internal.NodeFactories;

public class ExtendsNodeFactory : INodeFactory
{
    public int Priority => 100_000;

    public bool Handles(object? item, object? context = null)
    {
        if (item is IDictionary<string, object?> keyedDict && keyedDict.ContainsKey("$extends"))
        {
            return true;
        }

        if (item is IDictionary<object, object?> dict && dict.ContainsKey("$extends"))
        {
            return true;
        }

        return false;
    }

    public GraphNode CreateNode(
        Func<GraphNode, object?, object?, GraphNode> childrenFactory,
        GraphNode parent,
        object? item,
        object? context = null
    )
    {
        if (item is not IDictionary<string, object?> and not IDictionary<object, object?>)
        {
            throw new InvalidOperationException(); // TODO
        }

        var extends = string.Empty;

        if (item is IDictionary<string, object?> keyedDict)
        {
            extends = keyedDict["$extends"]?.ToString();
            keyedDict.Remove("$extends");
        }

        if (item is IDictionary<object, object?> dict)
        {
            extends = dict["$extends"]?.ToString();
            dict.Remove("$extends");
        }

        // TODO: Make parsing resilient
        var split = extends!.Split(".");

        var result = context is not null
            ? new KeyedExtendsNode(context.ToString()!, split[0], split[1])
            : new ExtendsNode(split[0], split[1]);

        var childNode = childrenFactory(result, item, context);

        return result with
        {
            Children = childNode.Children,
        };
    }
}
