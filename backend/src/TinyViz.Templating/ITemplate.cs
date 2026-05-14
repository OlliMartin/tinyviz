namespace TinyViz.Templating;

public interface ITemplate
{
    Dictionary<string, object?> Content { get; }
}
