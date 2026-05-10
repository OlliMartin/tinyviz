using Plotly.NET;
using TinyViz.Contracts.Model.GraphDescriptors;

namespace TinyViz.Renderer;

public interface IChartBuilder
{
    bool Handles(IGraphDescriptor graphDescriptor);

    Task<GenericChart> BuildAsync(IGraphDescriptor graphDescriptor, double? value, CancellationToken cancellationToken);
}
