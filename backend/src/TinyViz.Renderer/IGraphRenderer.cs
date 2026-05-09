using Plotly.NET;

namespace TinyViz.Renderer;

public interface IGraphRenderer
{
    Task<string> RenderAsync(GenericChart chart, int dimensions, CancellationToken cancellationToken = default);
}
