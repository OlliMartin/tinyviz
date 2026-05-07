namespace TinyViz.DataSource.Prometheus.Model.RestApi;

public class MetricLabels : Dictionary<string, string>
{
    public string Name => TryGetValue("__name__", out var name) ? name : "Unknown";
}
