using System.Text.Json.Serialization;

namespace TinyViz.DataSource.Prometheus.Model.RestApi;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ResultType
{
    Vector,
    Matrix,
}
