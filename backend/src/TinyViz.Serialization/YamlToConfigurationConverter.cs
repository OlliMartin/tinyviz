using System.ComponentModel.DataAnnotations;
using TinyViz.Contracts;
using TinyViz.Contracts.Model.ChartSpecification;
using TinyViz.Contracts.Model.Exceptions;
using TinyViz.Contracts.Model.GraphDescriptors;
using TinyViz.Serialization.Yaml;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace TinyViz.Serialization;

/// <summary>
/// Converts a YAML configuration into the <see cref="ChartDefinition"/> representation.
/// </summary>
public class YamlToConfigurationConverter() : IGraphConverter<string, ChartDefinition>
{
    private static readonly IDeserializer _yamlDeserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .WithNodeDeserializer(inner => new ComponentModelValidatorNodeDeserializer(inner), s => s.InsteadOf<ObjectNodeDeserializer>())
        .IgnoreUnmatchedProperties()
        .Build();

    public Task<IGraphDescriptor<ChartDefinition>> ConvertAsync(
        IGraphDescriptor<string> graphDescriptor,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var result = _yamlDeserializer.Deserialize<ChartDefinition>(graphDescriptor.Typed);
            return Task.FromResult<IGraphDescriptor<ChartDefinition>>(new ConfigurableGraphDescriptor(result));
        }
        catch (YamlException ex) when (ex.InnerException is ValidationException validationException)
        {
            throw new ConverterException("The deserialized YAML is not a valid graph descriptor.", validationException);
        }
        catch (YamlException ex)
        {
            throw new ConverterException("Could not deserialize provided YAML.", ex);
        }
    }
}
