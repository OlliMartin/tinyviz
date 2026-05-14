using TinyViz.Templating;

namespace Unit.TinyViz.Templating.Framework;

public class TestTemplate(Dictionary<string, object?> content) : ITemplate
{
    public Dictionary<string, object?> Content => content;
}
