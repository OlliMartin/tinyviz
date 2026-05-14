namespace TinyViz.Templating.Internal;

internal class DefaultTemplatingEngine : ITemplatingEngine
{
    public async Task<TTemplatable> RenderTemplateAsync<TTemplatable>(
        IEnumerable<ITemplateProvider> templateProviders,
        TTemplatable templatable,
        CancellationToken cancellationToken = default
    )
        where TTemplatable : ITemplatable
    {
        // Currently two entry points:
        // $extends: [Namespace.Name] <- !?!?!?
        // or inline -
        // values: ${Namespace.Name} <- covered in visitor

        foreach (var kvp in context)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return templatable;
            }

            await VisitAsync(kvp.Key, kvp.Value, cancellationToken);
        }

        return templatable;
    }

    private async Task VisitAsync(string node, object? children, CancellationToken cancellationToken = default)
    {
        switch (children)
        {
            case IList<object?> nestedList:
                // ??

                break;

            case IDictionary<string, object?> nestedMap:
                foreach (var kvp in nestedMap)
                {
                    await VisitAsync(kvp.Key, kvp.Value, cancellationToken);
                }

                break;
        }
    }
}
