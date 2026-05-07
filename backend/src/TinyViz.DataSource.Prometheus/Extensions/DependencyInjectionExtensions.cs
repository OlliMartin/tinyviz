using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyViz.Contracts;

namespace TinyViz.DataSource.Prometheus.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddPrometheusDataSource(IConfiguration configurationRoot)
        {
            serviceCollection
                .AddScoped<IDataSource, PrometheusDataSource>()
                .AddHttpClient(
                    "prom",
                    (provider, client) =>
                    {
                        client.BaseAddress = new("https://prom.acaad.dev", UriKind.Absolute);
                    }
                );

            return serviceCollection;
        }
    }
}
