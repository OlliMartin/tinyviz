using System.Diagnostics.CodeAnalysis;
using TinyViz.Contracts;
using TinyViz.Contracts.Model.ChartSpecification;
using TinyViz.Contracts.Model.Exceptions;
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

    [Fact]
    public async Task ShouldDeserializeAdditionalProperties()
    {
        const string typeName = "indicator";
        const string mode = "gauge";

        var gd = FromYaml(
            $"""
            chart:
              trace:
                typeName: {typeName}
                mode: {mode}
            """
        );

        var result = await _testSubject.ConvertAsync(gd, _ct);

        var casted = result.ShouldBeOfType<ConfigurableGraphDescriptor>();

        var expectedTrace = new TraceDescriptor
        {
            TypeName = typeName,
            ExtensionData = new() { ["mode"] = mode },
        };

        casted.Typed.Chart.Trace.ShouldBe(expectedTrace, TraceDescriptor.JsonComparer);
    }

    [Fact]
    public async Task ShouldDeserializeAdditionalPropertiesNested()
    {
        const string typeName = "indicator";
        const string mode = "gauge";

        var gd = FromYaml(
            $"""
            chart:
              trace:
                typeName: {typeName}
                mode: {mode}
                title:
                  text: 'CPU Busy'
            """
        );

        var result = await _testSubject.ConvertAsync(gd, _ct);

        var casted = result.ShouldBeOfType<ConfigurableGraphDescriptor>();

        var titleData = new Dictionary<string, object?> { ["text"] = "CPU Busy" };

        var expectedTrace = new TraceDescriptor
        {
            TypeName = typeName,
            ExtensionData = new() { ["mode"] = mode, ["title"] = titleData },
        };

        casted.Typed.Chart.Trace.ShouldBe(expectedTrace, TraceDescriptor.JsonComparer);
    }

    [Fact]
    public async Task ShouldDeserializeAdditionalPropertiesNestedList()
    {
        const string typeName = "indicator";
        const string mode = "gauge";

        var gd = FromYaml(
            $"""
            chart:
              trace:
                typeName: {typeName}
                mode: {mode}
                domain:
                  x:
                    - 0.1
                    - 1.0
                  y:
                    - 0.2
                    - 1.0
            """
        );

        var result = await _testSubject.ConvertAsync(gd, _ct);

        var casted = result.ShouldBeOfType<ConfigurableGraphDescriptor>();

        var domainData = new Dictionary<string, object?> { ["x"] = (List<double>)[0.1d, 1.0d], ["y"] = (List<double>)[0.2d, 1.0d] };

        var expectedTrace = new TraceDescriptor
        {
            TypeName = typeName,
            ExtensionData = new() { ["mode"] = mode, ["domain"] = domainData },
        };

        casted.Typed.Chart.Trace.ShouldBe(expectedTrace, TraceDescriptor.JsonComparer);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData(
        """
            chart:
              trace:
                typeNameMissing: yes
            """,
        "JSON deserialization for type 'TinyViz.Contracts.Model.ChartSpecification.TraceDescriptor' was missing required properties including: 'typeName'."
    )]
    public async Task ShouldThrowOnInvalidYaml(string invalidYaml, string? innerMessage = null)
    {
        var gd = FromYaml(invalidYaml);

        var act = () => _testSubject.ConvertAsync(gd, _ct);

        var ex = await act.ShouldThrowAsync<ConverterException>();

        if (innerMessage is not null)
        {
            ex.InnerException.ShouldNotBeNull();
            ex.InnerException.Message.ShouldBe(innerMessage);
        }
        else
        {
            ex.Message.ShouldBe("Could not deserialize provided YAML.");
        }
    }

    private IGraphDescriptor FromYaml([StringSyntax("yaml")] string yaml) => new YamlGraphDescriptor(yaml);
}
