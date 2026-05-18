using Argon;
using Integration.TinyViz.Templating.Framework;
using TinyViz.Templating.Internal;

namespace Integration.TinyViz.Templating;

public class GraphGenerationTests(TemplatingTestRuntime templatingTestRuntime)
{
    private static VerifySettings VerifySettings
    {
        get
        {
            var settings = new VerifySettings();

            settings.IgnoreMember(typeof(GraphNode), nameof(GraphNode.Parent));

            settings.UseDirectory("Snapshots");

            return settings;
        }
    }

    private DefaultTemplatingEngine TestSubject
    {
        get
        {
            var result = templatingTestRuntime.TemplatingEngine as DefaultTemplatingEngine;

            if (result is null)
            {
                Assert.Skip(
                    $"{nameof(templatingTestRuntime.TemplatingEngine)} is not assignable to {nameof(DefaultTemplatingEngine)}. Cannot create graph."
                );
            }

            return result;
        }
    }

    [Fact]
    public Task VerifyVerifySettings() => VerifyChecks.Run();

    [Fact]
    public async Task ExtendsKeyword_TopLevel_CreatesTypedExtendsNode()
    {
        var (target, yaml) = FromYaml(
            yamlString: """
            $extends: "Static.TopLevelTemplate"
            test: yes
            """
        );

        var graph = TestSubject.CreateGraph(target);

        await Verify(new TestCase(yaml, graph), VerifySettings);
    }

    [Fact]
    public async Task ExtendsKeyword_InList_CreatesTypedExtendsNode()
    {
        var (target, yaml) = FromYaml(
            yamlString: """
            list:
              - $extends: "Static.TopLevelTemplate"
                test: yes
            """
        );

        var graph = TestSubject.CreateGraph(target);

        await Verify(new TestCase(yaml, graph), VerifySettings);
    }

    [Fact]
    public async Task ExtendsKeyword_Nested_CreatesTypedExtendsNode()
    {
        var (target, yaml) = FromYaml(
            yamlString: """
            nested:
              $extends: "Static.TopLevelTemplate"
              test: yes
            """
        );

        var graph = TestSubject.CreateGraph(target);

        await Verify(new TestCase(yaml, graph), VerifySettings);
    }

    [Fact]
    public async Task TopLevelPrimitives()
    {
        var (target, yaml) = FromYaml(
            yamlString: """
            bool: true
            number: 1337
            string: 'Hello World'
            """
        );

        var graph = TestSubject.CreateGraph(target);

        await Verify(new TestCase(yaml, graph), VerifySettings);
    }

    [Fact]
    public async Task NestedPrimitives()
    {
        var (target, yaml) = FromYaml(
            yamlString: """
            nested:
              bool: true
              number: 1337
              string: 'Hello World'
            """
        );

        var graph = TestSubject.CreateGraph(target);

        await Verify(new TestCase(yaml, graph), VerifySettings);
    }

    [Fact]
    public async Task NestedPrimitivesKeyedWithNumber()
    {
        var (target, yaml) = FromYaml(
            yamlString: """
            nested:
              bool: true
              4711: 1337
              string: 'Hello World'
            """
        );

        var graph = TestSubject.CreateGraph(target);

        await Verify(new TestCase(yaml, graph), VerifySettings);
    }

    [Fact]
    public async Task NestedListPrimitives()
    {
        var (target, yaml) = FromYaml(
            yamlString: """
            nested:
              list:
                - true
                - 1337
                - 'Hello World'
            """
        );

        var graph = TestSubject.CreateGraph(target);

        await Verify(new TestCase(yaml, graph), VerifySettings);
    }

    [Fact]
    public async Task ListPrimitives()
    {
        var (target, yaml) = FromYaml(
            yamlString: """
            list:
              - true
              - 1337
              - 'Hello World'
            """
        );

        var graph = TestSubject.CreateGraph(target);

        await Verify(new TestCase(yaml, graph), VerifySettings);
    }

    [Fact]
    public async Task ListComplex()
    {
        var (target, yaml) = FromYaml(
            yamlString: """
            list:
              - key: bool
                value: true
              - key: number
                value: 1337
              - key: string
                value: 'Hello World'
            """
        );

        var graph = TestSubject.CreateGraph(target);

        await Verify(new TestCase(yaml, graph), VerifySettings);
    }

    [Fact]
    public async Task SingleVsMultiChildMismatch()
    {
        var (target, yaml) = FromYaml(
            yamlString: """
            singleChild: true
            multiChild:
              - true
              - false
            """
        );

        var graph = TestSubject.CreateGraph(target);

        await Verify(new TestCase(yaml, graph), VerifySettings);
    }

    private sealed record TestCase([property: JsonIgnore] string Yaml, [property: JsonProperty(Order = 100)] GraphNode Graph)
    {
        private static readonly string _spacer = string.Join("", Enumerable.Range(start: 0, count: 16).Select(_ => "#"));

        [JsonProperty(Order = 0)]
        public string TestCaseInput => $"{Environment.NewLine}{_spacer}{Environment.NewLine}{Yaml}{Environment.NewLine}{_spacer}";
    }
}
