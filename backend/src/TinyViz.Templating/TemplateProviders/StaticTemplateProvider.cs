namespace TinyViz.Templating.TemplateProviders;

public class StaticTemplateProvider(string @namespace, IList<ITemplate> templates) : ITemplateProvider
{
    public string Namespace => @namespace;

    public IList<ITemplate> Templates => templates;
}
