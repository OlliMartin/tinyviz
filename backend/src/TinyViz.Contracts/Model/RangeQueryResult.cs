namespace TinyViz.Contracts.Model;

public record RangeQueryResult(QueryDefinition Query, IReadOnlyList<TimeSeries> TimeSeries);
