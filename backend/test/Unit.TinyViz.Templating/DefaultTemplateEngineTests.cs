using System.Diagnostics.CodeAnalysis;
using TinyViz.Templating;
using TinyViz.Templating.Internal;
using TinyViz.Templating.TemplateProviders;
using Unit.TinyViz.Templating.Framework;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Unit.TinyViz.Templating;

[TestSubject(typeof(DefaultTemplatingEngine))]
public class DefaultTemplateEngineTests
{
    private static readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private static readonly IDeserializer _kvpDeserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    private static ITemplatingEngine TestSubject { get; } = new DefaultTemplatingEngine();

    [Fact]
    public async Task ShouldTemplateToplevelExtends()
    {
        var templateSource = SingleTemplateProvider(
            yamlString: """
            $namespace: 'Static'
            $name: 'TopLevelTemplate'
            toplevel: 'Hello World!'
            """
        );

        // Reference by "Namespace.Name"
        var target = FromYaml(
            yamlString: """
            $extends: "Static.TopLevelTemplate"
            test: yes
            """
        );

        var result = await TestSubject.RenderTemplateAsync([templateSource], target, _ct);

        // $name is skipped on purpose (even though plotly would not mind - I guess?)
        var expected = FromYaml(
            yamlString: """
            toplevel: 'Hello World!'
            test: yes
            """
        );

        result.ShouldBe(expected);
    }

    [Fact]
    public async Task ShouldTemplateInlineValues()
    {
        /*
          Template providers could define their "namespace" (DataSource) and specific templates they offer (Template.Name?)
        */

        var target = FromYaml(
            yamlString: """
            values: ${DataSource.Values}
            """
        );

        var expected = FromYaml(
            yamlString: """
            values: [1337]
            """
        );
    }

    private static ITemplatable FromYaml([StringSyntax("Yaml")] string yamlString)
    {
        var templateContentFromYaml = _kvpDeserializer.Deserialize<Dictionary<string, object?>>(yamlString);

        return new TestTemplatable(templateContentFromYaml);
    }

    private static StaticTemplateProvider SingleTemplateProvider([StringSyntax("Yaml")] string yamlString)
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
