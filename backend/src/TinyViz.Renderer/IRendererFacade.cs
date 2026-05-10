using TinyViz.Contracts.Model.GraphDescriptors;

namespace TinyViz.Renderer;

public interface IRendererFacade
{
    /// <summary>
    /// Matches the provided payload against the registered graph builders, builds the graph and renders it as PNG (base64 encoded).
    /// </summary>
    /// <param name="graphRepresentation">The graph descriptor to render. The <see cref="IChartBuilder"/> will be selected based on the underlying C# type.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The base64-encoded png representation.</returns>
    Task<string> RenderPngAsync(IGraphDescriptor graphRepresentation, CancellationToken cancellationToken);
}
