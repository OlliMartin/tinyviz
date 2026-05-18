namespace TinyViz.Templating;

public interface ITemplatingEngine
{
    Task<TTemplate> RenderTemplateAsync<TTemplate>(
        IEnumerable<ITemplateProvider> templateProviders,
        TTemplate templatable,
        CancellationToken cancellationToken = default
    )
        where TTemplate : ITemplate;
}
