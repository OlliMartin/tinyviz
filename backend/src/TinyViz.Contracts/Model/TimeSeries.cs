namespace TinyViz.Contracts.Model;

public record TimeSeries(MetricDescriptor Descriptor, IReadOnlyList<DataPoint> Values);
