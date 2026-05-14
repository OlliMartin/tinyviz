namespace TinyViz.Templating;

public interface ITemplatable
{
    Dictionary<string, object?> TemplateContext { get; }
}
