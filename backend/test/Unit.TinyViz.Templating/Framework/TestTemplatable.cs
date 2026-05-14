using TinyViz.Templating;

namespace Unit.TinyViz.Templating.Framework;

public class TestTemplatable(Dictionary<string, object?> templateContent) : ITemplatable
{
    public Dictionary<string, object?> TemplateContext => templateContent;
}
