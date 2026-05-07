namespace TinyViz.DataSource.Prometheus.Model.RestApi;

public record ApiData(ResultType ResultType, IReadOnlyList<PrometheusTimeSeries> Result);
