using TinyViz.Contracts;
using TinyViz.Contracts.Model.ChartSpecification;
using TinyViz.Contracts.Model.GraphDescriptors;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TinyViz.Serialization;

/// <summary>
/// Converts a YAML configuration into the <see cref="ChartDefinition"/> representation.
/// </summary>
public class YamlToConfigurationConverter() : IGraphConverter<string, ChartDefinition>
{
    private static IDeserializer yamlDeserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public Task<IGraphDescriptor<ChartDefinition>> ConvertAsync(
        IGraphDescriptor<string> graphDescriptor,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var result = yamlDeserializer.Deserialize<ChartDefinition>(graphDescriptor.Typed);
            return Task.FromResult<IGraphDescriptor<ChartDefinition>>(new ConfigurableGraphDescriptor(result));
        }
        catch (YamlException ex) // TODO
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}
