using System.Diagnostics.CodeAnalysis;
using TinyViz.Templating;
using TinyViz.Templating.TemplateProviders;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Shared.TinyViz.Testing.Framework;

public static class TemplateCreationUtility
{
    private static readonly IDeserializer _kvpDeserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    public static (ITemplate Template, string Yaml) FromYaml([StringSyntax("Yaml")] string yamlString)
    {
        var templateContentFromYaml = _kvpDeserializer.Deserialize<Dictionary<string, object?>>(yamlString);

        return (new TestTemplate(templateContentFromYaml), yamlString);
    }

    public static StaticTemplateProvider SingleTemplateProvider([StringSyntax("Yaml")] string yamlString)
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

        return new(@namespace, [new TestTemplate(provider),]);
    }
}
