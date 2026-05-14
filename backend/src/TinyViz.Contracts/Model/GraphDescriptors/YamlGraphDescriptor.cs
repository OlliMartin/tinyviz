namespace TinyViz.Contracts.Model.GraphDescriptors;

public record YamlGraphDescriptor(string Yaml) : IGraphDescriptor<string>
{
    public string Typed => Yaml;

    // TODO: Need to think about this.. On the one hand it doesn't make much sense to retrieve it from YAML,
    // on the other hand it violates the already tiny interface.
    // Maybe it would be worth to split GraphDescriptor into transport format and actual in-memory representation
    // For now.. We'll just roll with it.
    public QueryDefinition Query =>
        throw new InvalidOperationException($"Cannot retrieve query definition from {nameof(YamlGraphDescriptor)}.");
}
