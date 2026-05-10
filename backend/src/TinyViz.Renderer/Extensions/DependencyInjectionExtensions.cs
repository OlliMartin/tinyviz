using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TinyViz.Renderer.Builders;
using TinyViz.Renderer.Internal;
using TinyViz.Renderer.Renderers;

namespace TinyViz.Renderer.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddGraphRendering(IConfiguration configurationRoot)
        {
            serviceCollection.TryAddSingleton<IRendererFacade, DefaultRendererFacade>();

            serviceCollection.TryAddScoped<IChartBuilder, ConfigurationChartBuilder>();

            serviceCollection.TryAddKeyedScoped<IGraphRenderer, PngGraphRenderer>(DiConstants.Keyed.PngRenderer);

            return serviceCollection;
        }
    }
}
