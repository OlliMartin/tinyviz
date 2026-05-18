using Integration.TinyViz.Templating.Framework;
using TinyViz.Templating;
using TinyViz.Templating.Internal;

namespace Integration.TinyViz.Templating;

[TestSubject(typeof(DefaultTemplatingEngine))]
public class DefaultTemplateEngineTests(TemplatingTestRuntime testRuntime)
{
    private static readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private ITemplatingEngine TestSubject => testRuntime.TemplatingEngine;

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
        var (target, _) = FromYaml(
            yamlString: """
                        $extends: "Static.TopLevelTemplate"
                        test: yes
                        """
        );

        var result = await TestSubject.RenderTemplateAsync([templateSource,], target, _ct);

        // $name is skipped on purpose (even though plotly would not mind - I guess?)
        var expected = FromYaml(
            yamlString: """
                        toplevel: 'Hello World!'
                        test: yes
                        """
        );

        // result.ShouldBe(expected);
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
}
