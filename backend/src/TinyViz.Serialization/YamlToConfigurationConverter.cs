using System.Text.Json;
using TinyViz.Contracts;
using TinyViz.Contracts.Model.ChartSpecification;
using TinyViz.Contracts.Model.Exceptions;
using TinyViz.Contracts.Model.GraphDescriptors;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TinyViz.Serialization;

/// <summary>
///     Converts a YAML configuration into the <see cref="ChartDefinition" /> representation.
/// </summary>
public class YamlToConfigurationConverter : IGraphConverter<string, ChartDefinition>
{
    private static readonly JsonSerializerOptions _camelCaseJsonSettings = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private static readonly IDeserializer _yamlDeserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .IgnoreUnmatchedProperties()
        .Build();

    private static readonly ISerializer _jsonSerializer = new SerializerBuilder().JsonCompatible().Build();

    public Task<IGraphDescriptor<ChartDefinition>> ConvertAsync(
        IGraphDescriptor<string> graphDescriptor,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var jsonNode = _yamlDeserializer.Deserialize<Dictionary<string, object?>>(graphDescriptor.Typed);
            var jsonString = _jsonSerializer.Serialize(jsonNode);

            var result = JsonSerializer.Deserialize<ChartDefinition>(jsonString, _camelCaseJsonSettings);

            if (result is null)
            {
                throw new ConverterException("Could not deserialize provided YAML. Result object tree was null.");
            }

            return Task.FromResult<IGraphDescriptor<ChartDefinition>>(new ConfigurableGraphDescriptor(result));
        }
        catch (JsonException ex)
        {
            throw new ConverterException("The deserialized YAML is not a valid graph descriptor.", ex);
        }
        catch (YamlException ex)
        {
            throw new ConverterException("Could not deserialize provided YAML.", ex);
        }
    }
}
