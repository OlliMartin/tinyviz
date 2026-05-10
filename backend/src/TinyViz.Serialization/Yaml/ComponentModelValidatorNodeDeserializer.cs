using System.ComponentModel.DataAnnotations;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace TinyViz.Serialization.Yaml;

public class ComponentModelValidatorNodeDeserializer(INodeDeserializer nodeDeserializer) : INodeDeserializer
{
    public bool Deserialize(
        IParser reader,
        Type expectedType,
        Func<IParser, Type, object?> nestedObjectDeserializer,
        out object? value,
        ObjectDeserializer rootDeserializer
    )
    {
        if (nodeDeserializer.Deserialize(reader, expectedType, nestedObjectDeserializer, out value, rootDeserializer) is false)
        {
            return false;
        }

        if (value is null)
        {
            throw new YamlException("Expected a non-null value after deserialization, but found 'null'.");
        }

        var context = new ValidationContext(value, serviceProvider: null, items: null);
        Validator.ValidateObject(value, context, validateAllProperties: true);

        return false;
    }
}
