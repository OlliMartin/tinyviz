using Microsoft.Extensions.DependencyInjection;
using TinyViz.Contracts;
using TinyViz.Contracts.Model.GraphDescriptors;

namespace TinyViz.Renderer.Internal;

public class DefaultRendererFacade(
    IEnumerable<IChartBuilder> chartBuilders,
    [FromKeyedServices(DiConstants.Keyed.PngRenderer)] IGraphRenderer pngGraphRenderer,
    /* TODO: Workaround -> Cleanup */
    [FromKeyedServices("YamlToConfigConverter")] IGraphConverter graphConverter
) : IRendererFacade
{
    public async Task<string> RenderPngAsync(IGraphDescriptor graphRepresentation, CancellationToken cancellationToken = default)
    {
        // TODO: Workaround; Conversion chain can be calculated by extending the converter interface (and calculating shortest path with dijkstra?)
        var configurableGraph = await graphConverter.ConvertAsync(graphRepresentation, cancellationToken);

        var responsibleBuilders = chartBuilders.Where(cb => cb.Handles(configurableGraph)).ToList();

        switch (responsibleBuilders.Count)
        {
            case 0:
                throw new InvalidOperationException($"No graph renderer found for provided type '{configurableGraph.GetType().FullName}'.");

            case > 1:
                throw new InvalidOperationException(
                    $"Not exactly one graph renderer found for provided type '{configurableGraph.GetType().FullName}'."
                );
        }

        var builder = responsibleBuilders[index: 0];

        var chart = await builder.BuildAsync(configurableGraph, value: 0, cancellationToken);

        return await pngGraphRenderer.RenderAsync(chart, dimensions: 144, cancellationToken);
    }
}
