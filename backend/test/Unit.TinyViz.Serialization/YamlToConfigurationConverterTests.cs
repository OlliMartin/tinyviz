using System.Diagnostics.CodeAnalysis;
using TinyViz.Contracts;
using TinyViz.Contracts.Model.GraphDescriptors;
using TinyViz.Serialization;

namespace Unit.TinyViz.Serialization;

[TestSubject(typeof(YamlToConfigurationConverter))]
public class YamlToConfigurationConverterTests
{
    private static readonly CancellationToken _ct = TestContext.Current.CancellationToken;
    private readonly IGraphConverter _testSubject = new YamlToConfigurationConverter();

    [Fact]
    public async Task SanityCheck()
    {
        var gd = FromYaml(
            yaml: """
            chart:
              trace:
                typeName: indicator
            """
        );

        var result = await _testSubject.ConvertAsync(gd, _ct);

        var casted = result.ShouldBeOfType<ConfigurableGraphDescriptor>();

        casted.Typed.Chart.Trace.ShouldSatisfyAllConditions(td => td.TypeName.ShouldBe("indicator"));
    }

    private IGraphDescriptor FromYaml([StringSyntax("yaml")] string yaml) => new YamlGraphDescriptor(yaml);
}
