using Microsoft.Extensions.DependencyInjection;
using TinyViz.Contracts;

namespace TinyViz.Serialization.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddConverters() =>
            serviceCollection
                .AddSingleton<YamlToConfigurationConverter>()
                .AddKeyedSingleton<IGraphConverter, YamlToConfigurationConverter>(
                    DiConstants.Keyed.YamlToConfigConverter,
                    (sp, _) => sp.GetRequiredService<YamlToConfigurationConverter>()
                );
    }
}
