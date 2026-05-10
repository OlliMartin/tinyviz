using Microsoft.Extensions.DependencyInjection;
using TinyViz.Contracts.Model.GraphDescriptors;

namespace TinyViz.Renderer.Internal;

public class DefaultRendererFacade(
    IEnumerable<IChartBuilder> chartBuilders,
    [FromKeyedServices(DiConstants.Keyed.PngRenderer)] IGraphRenderer pngGraphRenderer
) : IRendererFacade
{
    public async Task<string> RenderPngAsync(IGraphDescriptor graphRepresentation, CancellationToken cancellationToken)
    {
        var responsibleBuilders = chartBuilders.Where(cb => cb.Handles(graphRepresentation)).ToList();

        switch (responsibleBuilders.Count)
        {
            case 0:
                throw new InvalidOperationException(
                    $"No graph renderer found for provided type '{graphRepresentation.GetType().FullName}'."
                );

            case > 1:
                throw new InvalidOperationException(
                    $"Not exactly one graph renderer found for provided type '{graphRepresentation.GetType().FullName}'."
                );
        }

        var builder = responsibleBuilders[index: 0];

        var chart = await builder.BuildAsync(graphRepresentation, value: 0, cancellationToken);

        return await pngGraphRenderer.RenderAsync(chart, dimensions: 144, cancellationToken);
    }
}
