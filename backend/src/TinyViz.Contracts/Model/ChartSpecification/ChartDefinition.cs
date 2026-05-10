namespace TinyViz.Contracts.Model.ChartSpecification;

[UsedImplicitly]
public record ChartDefinition
{
    public required ChartDescriptor Chart { get; init; }

    public LayoutDescriptor? Layout { get; init; }
}
