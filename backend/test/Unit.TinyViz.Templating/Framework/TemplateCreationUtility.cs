using System.Diagnostics.CodeAnalysis;
using TinyViz.Templating;
using TinyViz.Templating.TemplateProviders;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Unit.TinyViz.Templating.Framework;

internal static class TemplateCreationUtility
{
    private static readonly IDeserializer _kvpDeserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    internal static (ITemplate Template, string Yaml) FromYaml([StringSyntax("Yaml")] string yamlString)
    {
        var templateContentFromYaml = _kvpDeserializer.Deserialize<Dictionary<string, object?>>(yamlString);

        return (new TestTemplate(templateContentFromYaml), yamlString);
    }

    internal static StaticTemplateProvider SingleTemplateProvider([StringSyntax("Yaml")] string yamlString)
    {
        var provider = _kvpDeserializer.Deserialize<Dictionary<string, object?>>(yamlString);

        var namespaceUntyped = provider["$namespace"];
        var nameUntyped = provider["$name"];

        if (namespaceUntyped is not string @namespace || nameUntyped is not string name)
        {
            throw new InvalidOperationException();
        }

        provider.Remove("$namespace");
        provider.Remove("$name");

        return new(@namespace, [new TestTemplate(provider)]);
    }
}
