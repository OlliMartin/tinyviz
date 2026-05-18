using System.Diagnostics.CodeAnalysis;
using Integration.TinyViz.Templating.Framework;
using TinyViz.Templating.Internal;

namespace Integration.TinyViz.Templating;

public class GraphDeserializationSerializationEquivalencyTests(TemplatingTestRuntime templatingTestRuntime)
{
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

    public static IEnumerable<TheoryDataRow<string>> EquivalencyTestCases
    {
        get
        {
            yield return _("");

            yield return _("bool: true");
            yield return _("int: 0");
            yield return _("decimal: 1.4");
            yield return _("string: Hello World");
            yield return _("quotedString: 'Hello World'");

            yield return _("emptyObject: { }");
            yield return _("emptyList: [ ]");
        }
    }

    [Theory]
    [MemberData(nameof(EquivalencyTestCases))]
    public void EquivalencyTableTests(string testCaseYaml)
    {
        var (target, yaml) = FromYaml(testCaseYaml);

        var deserializedGraph = TestSubject.CreateGraph(target);
        var serializedGraph = "";

        serializedGraph.ShouldBe(yaml);
    }

    private static string _([StringSyntax("yaml")] string yaml) => yaml;
}
