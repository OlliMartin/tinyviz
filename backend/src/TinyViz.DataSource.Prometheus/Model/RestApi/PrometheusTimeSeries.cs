using Riok.Mapperly.Abstractions;

namespace TinyViz.DataSource.Prometheus.Model.RestApi;

public record PrometheusTimeSeries(MetricLabels Metric, IReadOnlyList<PrometheusDataPoint> Values);
