using System.Diagnostics.CodeAnalysis;
using Integration.TinyViz.Templating.Framework;
using TinyViz.Templating.Internal;
using YamlDotNet.Serialization;

namespace Integration.TinyViz.Templating;

public partial class GraphDeserializationSerializationSymmetryTests(TemplatingTestRuntime templatingTestRuntime)
{
    private readonly ISerializer _stringSerializer = new SerializerBuilder().WithQuotingNecessaryStrings().WithIndentedSequences().Build();

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

            yield return _("objectEmpty: {}");

            yield return _("listEmpty: []");

            yield return _(
                yaml: """
                listBool:
                  - true
                  - false
                """
            );

            yield return _(
                yaml: """
                listComplex:
                  - key: Key1
                    value: Value1
                  - key: Key2
                    value: Value2
                """
            );

            yield return _(
                yaml: """
                listNumber:
                  - 1337
                  - 4711
                  - 6969
                """
            );

            yield return _(
                yaml: """
                mixedGraph:
                  scalar: Value
                  emptyObject: {}
                  emptyList: []
                  listOfObjects:
                    - name: Item1
                      enabled: true
                    - name: Item2
                      enabled: false
                """
            );

            yield return _(
                yaml: """
                yamlSensitiveStrings:
                  colon: 'key: value'
                  hash: 'value # not a comment'
                  brackets: '[a, b]'
                  spaces: "  padded  "
                """
            );
        }
    }

    [Theory]
    [MemberData(nameof(EquivalencyTestCases))]
    [MemberData(nameof(Sonnet47))]
    [MemberData(nameof(GeminiThinking))]
    public void EquivalencyTableTests(string testCaseYaml)
    {
        var (target, yaml) = FromYaml(testCaseYaml);

        var deserializedGraph = TestSubject.CreateGraph(target);
        var targetDictionary = new Dictionary<string, object?>();
        deserializedGraph.SerializeInto(targetDictionary);

        var serializedGraph = _stringSerializer.Serialize(targetDictionary);

        serializedGraph.Trim().ShouldBe(yaml, StringCompareShould.IgnoreLineEndings);
    }

    private static string _([StringSyntax("yaml")] string yaml) => yaml;
}
