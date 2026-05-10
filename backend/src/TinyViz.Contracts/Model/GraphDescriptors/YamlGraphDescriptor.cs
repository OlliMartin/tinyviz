namespace TinyViz.Contracts.Model.GraphDescriptors;

public record YamlGraphDescriptor(string Yaml) : IGraphDescriptor<string>
{
    public string Typed => Yaml;
}
