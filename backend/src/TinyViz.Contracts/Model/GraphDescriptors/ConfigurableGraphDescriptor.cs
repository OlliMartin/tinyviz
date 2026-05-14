using TinyViz.Contracts.Model.ChartSpecification;

namespace TinyViz.Contracts.Model.GraphDescriptors;

public record ConfigurableGraphDescriptor(ChartDefinition ChartDefinition) : IGraphDescriptor<ChartDefinition>
{
    public ChartDefinition Typed => ChartDefinition;

    public QueryDefinition Query => ChartDefinition.Chart.Data;
}
