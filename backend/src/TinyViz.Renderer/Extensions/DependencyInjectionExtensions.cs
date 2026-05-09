using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyViz.Renderer.Builders;
using TinyViz.Renderer.Renderers;

namespace TinyViz.Renderer.Extensions;

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection AddGraphRendering(IConfiguration configurationRoot) =>
            serviceCollection.AddScoped<IChartBuilder, JsonChartBuilder>().AddSingleton<IGraphRenderer, PngGraphRenderer>();
    }
}
