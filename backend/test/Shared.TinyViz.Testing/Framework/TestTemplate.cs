using TinyViz.Templating;

namespace Shared.TinyViz.Testing.Framework;

public class TestTemplate(Dictionary<string, object?> content) : ITemplate
{
    public Dictionary<string, object?> Content => content;
}
