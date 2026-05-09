using Plotly.NET;

namespace TinyViz.Renderer;

public interface IChartBuilder
{
    Task<GenericChart> BuildAsync(double? value, CancellationToken cancellationToken);
}
