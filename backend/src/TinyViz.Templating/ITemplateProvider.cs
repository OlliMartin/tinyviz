namespace TinyViz.Templating;

public interface ITemplateProvider
{
    string Namespace { get; }

    IList<ITemplate> Templates { get; }
}
