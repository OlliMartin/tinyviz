namespace TinyViz.Contracts.Model.ChartSpecification;

[UsedImplicitly]
public record ChartDescriptor
{
    public required TraceDescriptor Trace { get; init; }

    public required QueryDefinition Data { get; init; }
}
