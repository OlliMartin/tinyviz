using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using TinyViz.Contracts;
using TinyViz.Contracts.Model;
using TinyViz.DataSource.Prometheus.Model.Mapper;
using TinyViz.DataSource.Prometheus.Model.RestApi;

namespace TinyViz.DataSource.Prometheus;

public class PrometheusDataSource(IHttpClientFactory httpClientFactory) : IDataSource
{
    private const string RangeEndpoint = "api/v1/query_range";

    public async Task<RangeQueryResult> QueryRangeAsync(QueryDefinition queryDefinition, CancellationToken cancellationToken = default)
    {
        using var httpClient = httpClientFactory.CreateClient(queryDefinition.DataSourceName);

        using var httpResponse = await httpClient.GetAsync(BuildRangeQuery(queryDefinition), cancellationToken);

        httpResponse.EnsureSuccessStatusCode();

        var deserialized = await httpResponse.Content.ReadFromJsonAsync<ApiResponse>(cancellationToken);

        if (deserialized is null)
        {
            throw new JsonException();
        }

        return new(queryDefinition, deserialized.Data.Result.Select(ts => ts.ToContract()).ToList().AsReadOnly());
    }

    private string BuildRangeQuery(QueryDefinition queryDefinition)
    {
        var startTime = (DateTimeOffset.UtcNow - TimeSpan.FromHours(hours: 6)).ToUnixTimeMilliseconds() / 1000d;
        var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000d;

        // TODO: This is a very stupid implementation; But ok for a poc.
        var keyValuePairs = new Dictionary<string, string>
        {
            ["query"] = UrlEscape(queryDefinition.Query),
            ["step"] = "60", // In seconds
            ["start"] = startTime.ToString("F3"),
            ["end"] = endTime.ToString("F3"),
        };

        var queryString = string.Join("&", keyValuePairs.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        return $"{RangeEndpoint}?{queryString}";
    }

    private static string UrlEscape(string raw) => HttpUtility.UrlEncode(raw);
}
