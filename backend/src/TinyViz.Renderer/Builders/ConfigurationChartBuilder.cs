using Plotly.NET;
using TinyViz.Contracts.Model.ChartSpecification;
using TinyViz.Contracts.Model.GraphDescriptors;

namespace TinyViz.Renderer.Builders;

public class ConfigurationChartBuilder : IChartBuilder<ConfigurableGraphDescriptor, ChartDefinition>
{
    public Task<GenericChart> BuildAsync(ConfigurableGraphDescriptor descriptor, double? value, CancellationToken cancellationToken)
    {
        var chartDefinition = descriptor.ChartDefinition;
        var trace = new Trace(chartDefinition.Chart.Trace.TypeName);
        trace.SetValue("type", chartDefinition.Chart.Trace.TypeName);

        foreach (var kvp in chartDefinition.Chart.Trace.GetPrimitiveExtensionData())
        {
            trace.SetValue(kvp.Key, kvp.Value);
        }

        var layout = new Layout();

        foreach (var kvp in chartDefinition.Layout?.GetPrimitiveExtensionData() ?? [])
        {
            layout.SetValue(kvp.Key, kvp.Value);
        }

        var chart = GenericChart.ofTraceObject(useDefaults: false, trace).WithLayout(layout);

        return Task.FromResult(chart);
    }
}
