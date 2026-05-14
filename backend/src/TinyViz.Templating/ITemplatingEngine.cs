namespace TinyViz.Templating;

public interface ITemplatingEngine
{
    Task<TTemplatable> RenderTemplateAsync<TTemplatable>(
        IEnumerable<ITemplateProvider> templateProviders,
        TTemplatable templatable,
        CancellationToken cancellationToken = default
    )
        where TTemplatable : ITemplatable;
}
