using System.Diagnostics.CodeAnalysis;
using Integration.TinyViz.Templating.Framework;
using TinyViz.Templating.Internal;
using YamlDotNet.Serialization;

namespace Integration.TinyViz.Templating;

public class GraphDeserializationSerializationEquivalencyTests(TemplatingTestRuntime templatingTestRuntime)
{
    private readonly ISerializer stringSerializer = new SerializerBuilder().Build();

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
            yield return _("{}");

            yield return _("bool: true");
            yield return _("int: 0");
            yield return _("decimal: 1.4");
            yield return _("string: Hello World");

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
        var targetDictionary = new Dictionary<string, object?>();
        deserializedGraph.SerializeInto(targetDictionary);

        var serializedGraph = stringSerializer.Serialize(targetDictionary);

        serializedGraph.Trim().ShouldBe(yaml, StringCompareShould.IgnoreLineEndings);
    }

    private static string _([StringSyntax("yaml")] string yaml) => yaml;
}
