using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyViz.DataSource.Prometheus.Model.RestApi;

[JsonConverter(typeof(DataPointConverter))]
public record PrometheusDataPoint(double Timestamp, string Value)
{
    internal sealed class DataPointConverter : JsonConverter<PrometheusDataPoint>
    {
        public override PrometheusDataPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Prometheus returns: [timestamp, "value"]
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            reader.Read();
            var timestamp = reader.GetDouble();

            reader.Read();
            var value = reader.GetString();

            reader.Read(); // Consume EndArray
            return new(timestamp, value);
        }

        public override void Write(Utf8JsonWriter writer, PrometheusDataPoint value, JsonSerializerOptions options) =>
            throw new NotSupportedException();
    }
}
