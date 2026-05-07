using Plotly.NET;

namespace TinyViz.Renderer;

public interface IGraphRenderer
{
    Task<string> RenderAsync(GenericChart chart, CancellationToken cancellationToken = default);
}
