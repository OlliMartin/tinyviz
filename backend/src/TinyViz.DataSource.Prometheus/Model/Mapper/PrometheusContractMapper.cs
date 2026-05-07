using Riok.Mapperly.Abstractions;
using TinyViz.Contracts.Model;
using TinyViz.DataSource.Prometheus.Model.RestApi;

namespace TinyViz.DataSource.Prometheus.Model.Mapper;

[Mapper]
public static partial class PrometheusContractMapper
{
    [MapperIgnoreSource(nameof(MetricLabels.Capacity))]
    [MapperIgnoreSource(nameof(MetricLabels.Comparer))]
    [MapperIgnoreSource(nameof(MetricLabels.Count))]
    [MapperIgnoreSource(nameof(MetricLabels.Keys))]
    [MapperIgnoreSource(nameof(MetricLabels.Values))]
    public static partial MetricDescriptor ToContract(this MetricLabels metricLabels);

    public static partial DataPoint ToContract(this PrometheusDataPoint prometheusDataPoint);

    public static partial IReadOnlyList<DataPoint> ToContract(this IReadOnlyList<PrometheusDataPoint> prometheusDataPoints);

    public static TimeSeries ToContract(this PrometheusTimeSeries prometheusTimeSeries) =>
        new(prometheusTimeSeries.Metric.ToContract(), prometheusTimeSeries.Values.ToContract());
}
